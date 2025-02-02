using BackEndNotes.Collections;
using BackEndNotes.Dto;
using System.Reflection;
using BackEndNotes.Interfaces;
using BackEndNotes.Models;
using BackEndNotes.Models.Database;
using BackEndNotes.Services;
using BackEndNotes.Utils;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.OpenApi.Models;
using MongoDB.Driver.Core.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using System.Text.Json;
using BackEndNotes.Dto.Usuarios;
using BackEndNotes.Controllers;
using BackEndNotes.Models.Notes;
using BackEndNotes.Dto.Notes;
using BackEndNotes.Interfaces.Principals;

var builder = WebApplication.CreateBuilder(args);

// Add service database to Mongo
builder.Services.Configure<DatabaseModel>(builder.Configuration.GetSection("ConnectionStrings"));

// Add service of routes 
builder.Services.AddScoped<Context>();
builder.Services.AddScoped<ManejoPasswords>();
builder.Services.AddSingleton<Token>();

// Enlaces para notificacion 
builder.Services.AddScoped<INotification<MailModel>, NotificationMail>();
builder.Services.AddScoped<NotificationMail>();
// Enlaces para sesion de usuario 
builder.Services.AddScoped<ISessionUser<UserModel, PasswordDto, MailModel>, SessionCollection>();
builder.Services.AddScoped<SessionCollection>();
// Enlaces para rutas de usuario 
builder.Services.AddScoped<IUsuario<UpdateUserDto, UsuarioDataDto>, UsuarioCollection>();

// Enlaces para rutas de Notes
builder.Services.AddScoped<INotes<NotesModel, UpdateNoteDto>, NotesCollection>();
builder.Services.AddScoped<NotesCollection>();
builder.Services.AddScoped<NotesService>();
// Enlaces para Correo
builder.Services.AddScoped<IViewOne<UserModel>, MailCollection>();
builder.Services.AddScoped<MailCollection>();

// Files services
builder.Services.AddScoped<MailService>();
builder.Services.AddScoped<SessionService>();
builder.Services.AddScoped<MailService>();
builder.Services.AddScoped<UsuarioService>();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Plantas API",
        Version = "v1",
        Description = "Esta API REST permite la creación, lectura, actualización y eliminación de notas para una aplicación de gestión de notas. Está diseñada para proporcionar una manera sencilla y eficiente de que los usuarios gestionen sus notas personales, con soporte para la organización de las mismas por categorías o etiquetas, y para la sincronización entre diferentes dispositivos.",
        TermsOfService = new Uri("https://example.com/license"),
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
        Description = "Ingrese el token JWT en el formato: Bearer {token} para poder acceder a las rutas que requerie autenticacion",
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
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
    else
    {
        Console.WriteLine($"Advertencia: No se encontró el archivo XML en {xmlPath}");
    }
});

// Se agrega configuracion del acess token
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
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

            var response = new ResponseDto { Message = "No autorizado. Token invalido o expirado." };
            var jsonResponse = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(jsonResponse);
        }
    };
});

builder.Services.AddAuthorization();

builder.Services.AddControllers();

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("PermitirTodo");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
