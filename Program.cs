using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Notas_Back.Dto;
using Notas_Back.Interfaces;
using Notas_Back.Models;
using Notas_Back.Repositories;
using Notas_Back.Services;
using System.Buffers;
using System.Reflection;
using System.Text;
using System.Text.Json;
using Serilog;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Monitoreo de la aplicacion  using Serilog; using Microsoft.Extensions.Logging;
Log.Logger = new LoggerConfiguration().WriteTo.File("logs/myapp.txt", rollingInterval: RollingInterval.Day).CreateLogger();
// builder.Logging.ClearProviders();
builder.Logging.AddSerilog();


// Add services to the container.
// Agrega policía cors 
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTodo", builder =>
    {
        builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.Configure<MongoConections>(builder.Configuration.GetSection("MongoDb"));

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Notes API Rest",
        Description = "Esta API REST permite la creación, lectura, actualización y eliminación de notas para una aplicación de gestión de notas. Está diseñada para proporcionar una manera sencilla y eficiente de que los usuarios gestionen sus notas personales, con soporte para la organización de las mismas por categorías o etiquetas, y para la sincronización entre diferentes dispositivos.",
        // TermsOfService = new Uri(""),
        Contact = new OpenApiContact
        {
            Name = "Gustavo Bernal",
            Url = new Uri("https://folio-three-inky.vercel.app/")
        },
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://example.com/license")
        }
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Ingrese el token JWT en el formato: Bearer {token} para poder acceder a las rutas que requerien autenticacion ",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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

    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddControllers();
builder.Services.AddScoped<UsuariosService>();
builder.Services.AddScoped<NotasService>();
builder.Services.AddScoped<EnviarCorreo>();
builder.Services.AddScoped<generaToken>();
builder.Services.AddScoped<ManejoContraseñas>();
builder.Services.AddScoped<Context>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Se agrega configuracion del acess token
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
    // varify if token valid
    options.Events = new JwtBearerEvents
    {
        OnChallenge = context =>
        {
            context.HandleResponse(); // Evitar el manejo predeterminado
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;

            var response = new NoData { status = 401, mensaje = "No autorizado. Token invalido o expirado." };
            var jsonResponse = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(jsonResponse);
        }
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseSwaggerUI(options => // UseSwaggerUI is called only in Development.
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
        options.DocumentTitle = "My API Documentation";
    });
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseSwagger(options =>
{
    options.SerializeAsV2 = true;
});

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.UseStaticFiles();

app.MapControllers();

app.Run();
