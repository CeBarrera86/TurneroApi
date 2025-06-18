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

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Information);

// --- SERVICIOS ---
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

builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IClienteRemotoService, ClienteRemotoService>();
builder.Services.AddScoped<IEstadoService, EstadoService>();
builder.Services.AddScoped<IHistorialService, HistorialService>();
builder.Services.AddScoped<IMostradorService, MostradorService>();
builder.Services.AddScoped<IPuestoService, PuestoService>();
builder.Services.AddScoped<IRolService, RolService>();
builder.Services.AddScoped<ISectorService, SectorService>();
builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddScoped<ITurnoService, TurnoService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.\n\nExample: \"Bearer 12345abcdef\"",
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

// Configuración de TurneroDbContext (para la base de datos principal MySQL)
var turneroConnectionString = builder.Configuration.GetConnectionString("TurneroDb");
builder.Services.AddDbContext<TurneroDbContext>(options =>
    options.UseMySql(turneroConnectionString, ServerVersion.AutoDetect(turneroConnectionString))
);
// Configuración de GeaSeguridadDbContext (para la base de datos externa SQL Server)
var geaSeguridadConnectionString = builder.Configuration.GetConnectionString("GeaSeguridadDb");
builder.Services.AddDbContext<GeaSeguridadDbContext>(options =>
    options.UseSqlServer(geaSeguridadConnectionString));

// Configuración de GeaCorpicoDbContext (para la base de datos GeaCorpico SQL Server)
var geaCorpicoConnectionString = builder.Configuration.GetConnectionString("GeaCorpicoDb");
builder.Services.AddDbContext<GeaCorpicoDbContext>(options =>
    options.UseSqlServer(geaCorpicoConnectionString));

// --- Configuración de Autenticación JWT ---
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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is not configured in appsettings.json.")))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    // Configuración para producción
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();