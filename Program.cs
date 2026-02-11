using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TurneroApi.Data;
using TurneroApi.Hubs;
using TurneroApi.Interfaces.GeaPico;
using TurneroApi.Mappings;
using TurneroApi.Services;
using TurneroApi.Services.GeaPico;
using TurneroApi.Config;
using TurneroApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

// --- Logging ---
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Information);

// --- Controllers ---
var mvcBuilder = builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
  options.InvalidModelStateResponseFactory = context =>
  {
    var problemDetails = new ValidationProblemDetails(context.ModelState)
    {
      Status = StatusCodes.Status400BadRequest,
      Title = "Uno o más errores de validación ocurrieron.",
      Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
    };
    return new BadRequestObjectResult(problemDetails);
  };
});

// --- FluentValidation ---
mvcBuilder.AddFluentValidation(fv =>
  fv.RegisterValidatorsFromAssemblyContaining<TurneroApi.Validation.Fluent.DtoValidators>());

// --- ProblemDetails & ExceptionHandler ---
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// --- Swagger ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
  c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
  {
    Name = "Authorization",
    Type = SecuritySchemeType.ApiKey,
    Scheme = "Bearer",
    BearerFormat = "JWT",
    In = ParameterLocation.Header,
    Description = "JWT Authorization header usando el esquema Bearer. Ejemplo: \"Bearer 12345abcdef\""
  });
  c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            }, new string[] {}
        }
    });
});

  // --- SignalR ---
  builder.Services.AddSignalR();

// --- AutoMapper ---
builder.Services.AddAutoMapper(typeof(MappingProfile));

// --- CORS ---
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();
builder.Services.AddCors(options =>
{
  options.AddDefaultPolicy(policy =>
  {
    policy.WithOrigins(allowedOrigins)
      .AllowAnyHeader()
      .AllowAnyMethod()
      .AllowCredentials();
  });
});


// --- Servicios ---
builder.Services.AddScoped<IGeaSeguridadService, GeaSeguridadService>();

builder.Services.AddTurneroServices();

// --- Conexiones a bases de datos ---
var turneroConnectionString = builder.Configuration.GetConnectionString("TurneroDb");
Console.WriteLine($"🌍 Entorno activo: {builder.Environment.EnvironmentName}");

builder.Services.AddDbContext<TurneroDbContext>(options =>
{
  options.UseMySql(turneroConnectionString, ServerVersion.AutoDetect(turneroConnectionString));

  if (builder.Environment.IsDevelopment())
  {
    options.EnableSensitiveDataLogging();
    options.LogTo(Console.WriteLine, LogLevel.Information);
  }
});

var geaSeguridadConnectionString = builder.Configuration.GetConnectionString("GeaSeguridadDb");
builder.Services.AddDbContext<GeaSeguridadDbContext>(options =>
    options.UseSqlServer(geaSeguridadConnectionString));

var geaCorpicoConnectionString = builder.Configuration.GetConnectionString("GeaCorpicoDb");
builder.Services.AddDbContext<GeaCorpicoDbContext>(options =>
    options.UseSqlServer(geaCorpicoConnectionString));

// --- JWT ---
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
      options.TokenValidationParameters = new TokenValidationParameters
      {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
              builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is not configured.")
          ))
      };

      options.Events = new JwtBearerEvents
      {
        OnMessageReceived = context =>
        {
          var accessToken = context.Request.Query["access_token"];
          var path = context.HttpContext.Request.Path;

          if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs/tickets"))
          {
            context.Token = accessToken;
          }

          return Task.CompletedTask;
        }
      };
    });

// --- Autorización dinámica basada en permisos ---
builder.Services.AddSingleton<IAuthorizationPolicyProvider, DynamicAuthorizationPolicyProvider>();

// --- Política personalizada para acceso desde el Tótem ---
var totemIp = builder.Configuration["TotemSettings:Ip"];
var totemApiKey = builder.Configuration["TotemSettings:ApiKey"];

builder.Services.AddAuthorization(options =>
{
  options.AddPolicy("TotemAccess", policy =>
      policy.RequireAssertion(context =>
      {
        var httpContext = context.Resource as HttpContext;
        if (httpContext == null) return false;

        var remoteIp = httpContext.Connection.RemoteIpAddress?.ToString();
        var apiKey = httpContext.Request.Headers["X-Api-Key"].FirstOrDefault();

        return remoteIp == totemIp && apiKey == totemApiKey;
      }));
});

// --- Rutas NAS ---
builder.Services.Configure<RutasConfig>(builder.Configuration.GetSection("Rutas"));

// --- Build ---
var app = builder.Build();

// --- Pipeline HTTP ---
if (app.Environment.IsDevelopment())
{
  app.UseDeveloperExceptionPage();
}
else
{
  app.UseExceptionHandler();
  app.UseHsts();
}

app.UseRouting();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

// --- Swagger condicional ---
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.MapControllers();
app.MapHub<TicketsHub>("/hubs/tickets");
app.Run();