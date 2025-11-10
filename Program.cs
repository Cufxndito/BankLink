using BankLink.Data;
using BankLink.Auth;
using Microsoft.EntityFrameworkCore;
using BankLink.Services.Interfaces;
using BankLink.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

// =====================================
// 🔹 CONFIGURACIÓN DE KESTREL (SERVIDOR)
// =====================================
// Esto permite que tu API escuche conexiones desde cualquier IP de la red local,
// no solo desde "localhost".
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5071); // Escucha en todas las interfaces, puerto 5071
});

// ===============================
// 🔹 CONFIGURACIÓN BASE DE DATOS
// ===============================
builder.Services.AddDbContext<BankLinkContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ===============================
// 🔹 INYECCIÓN DE DEPENDENCIAS
// ===============================
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IBancoExternoService, BancoExternoService>();
builder.Services.AddHttpClient<ITransferenciaService, TransferenciaService>();
builder.Services.AddScoped<IMovimientoService, MovimientoService>();
builder.Services.AddScoped<ICuentaService, CuentaService>();

// ===============================
// 🔹 CONFIGURACIÓN DE CORS
// ===============================
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTodo", policy =>
    {
        policy.AllowAnyOrigin()    // Permitir cualquier IP o dominio
              .AllowAnyHeader()    // Permitir cualquier encabezado (Authorization, X-API-KEY)
              .AllowAnyMethod();   // Permitir todos los métodos HTTP
    });
});

// ===============================
// 🔹 CONTROLADORES Y FORMATO JSON
// ===============================
builder.Services.AddControllers()
    .AddJsonOptions(x =>
    {
        x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        x.JsonSerializerOptions.WriteIndented = true;
    });

// ===============================
// 🔹 SWAGGER (con autenticación integrada)
// ===============================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // --- Token interno (Bearer) ---
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Ingrese el token interno en el formato: Bearer TOKEN-DEMO-12345"
    });

    // --- API Key externa (para otros bancos) ---
    c.AddSecurityDefinition("X-API-KEY", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "X-API-KEY",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Clave de seguridad para integraciones entre bancos (por ejemplo: mi-clave-bkl-12345)"
    });

    // --- Aplicar los esquemas de seguridad ---
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        },
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "X-API-KEY"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddOpenApi();

var app = builder.Build();

// ===============================
// 🔹 MIDDLEWARES
// ===============================

// Autenticación (tokens internos + API Keys externas)
app.UseMiddleware<AuthMiddleware>();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

// Redirección HTTPS
app.UseHttpsRedirection();

// ✅ Activar CORS (debe ir antes de UseAuthorization)
app.UseCors("PermitirTodo");

// Autorización y controladores
app.UseAuthorization();
app.MapControllers();

// Ejecutar aplicación
app.Run();
