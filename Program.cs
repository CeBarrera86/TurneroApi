using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TurneroApi.Data;
using TurneroApi.Interfaces;
using TurneroApi.Interfaces.GeaPico;
using TurneroApi.Mappings;
using TurneroApi.Services;
using TurneroApi.Services.GeaPico;
using TurneroApi.Services.Mocks;
using TurneroApi.Config;

var builder = WebApplication.CreateBuilder(args);

// --- Logging ---
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Information);

// --- Controllers ---
builder.Services.AddControllers(options =>
{
}).ConfigureApiBehaviorOptions(options =>
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
    Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token.\n\nExample: \"Bearer 12345abcdef\"",
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
            },
            new string[] {}
        }
    });
});

// --- AutoMapper ---
builder.Services.AddAutoMapper(typeof(MappingProfile));

// --- CORS ---
builder.Services.AddCors(options =>
{
  options.AddDefaultPolicy(policy =>
  {
    policy.WithOrigins("http://localhost:5173", "http://172.16.14.87:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
  });
});

// --- Servicios ---
var geaMode = builder.Configuration["GeaSettings:Modo"];
if (geaMode == "Mock")
{
  builder.Services.AddScoped<IGeaSeguridadService, MockGeaSeguridadService>();
  builder.Logging.AddFilter("TurneroApi.Services.Mocks.MockGeaSeguridadService", LogLevel.Information);
}
else
{
  builder.Services.AddScoped<IGeaSeguridadService, GeaSeguridadService>();
}

builder.Services.AddScoped<ContenidoMappingAction>();
builder.Services.AddScoped<IArchivoService, ArchivoService>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IClienteRemotoService, ClienteRemotoService>();
builder.Services.AddScoped<IContenidoService, ContenidoService>();
builder.Services.AddScoped<IEstadoService, EstadoService>();
builder.Services.AddScoped<IHistorialService, HistorialService>();
builder.Services.AddScoped<IMostradorService, MostradorService>();
builder.Services.AddScoped<IPuestoService, PuestoService>();
builder.Services.AddScoped<IRolService, RolService>();
builder.Services.AddScoped<ISectorService, SectorService>();
builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddScoped<ITurnoService, TurnoService>();
builder.Services.AddScoped<IUrlBuilderService, UrlBuilderService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

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
    });

builder.Services.AddAuthorization();

// --- Rutas NAS ---
builder.Services.Configure<RutasConfig>(builder.Configuration.GetSection("Rutas"));

// --- Build ---
var app = builder.Build();

// --- Diagnóstico de conexión ---
using (var scope = app.Services.CreateScope())
{
  var dbContext = scope.ServiceProvider.GetRequiredService<TurneroDbContext>();
  if (dbContext.Database.CanConnect())
  {
    Console.WriteLine("✅ Conexión a la base de datos MySQL exitosa.");
  }
  else
  {
    Console.WriteLine("❌ Falló la conexión a la base de datos MySQL.");
  }
}

// --- Pipeline HTTP ---
if (app.Environment.IsDevelopment())
{
  app.UseDeveloperExceptionPage();
}
else
{
  app.UseExceptionHandler("/Error");
  app.UseHsts();
}

// ⚠️ HTTPS redirection solo si lo necesitás
// app.UseHttpsRedirection();


app.UseRouting();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

// --- Swagger condicional ---
if (app.Environment.IsDevelopment() || builder.Configuration.GetValue<bool>("EnableSwagger"))
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.MapControllers();
app.Run();