using BackEndNotes.Collections;
using BackEndNotes.Interfaces;
using BackEndNotes.Models;
using BackEndNotes.Models.Database;
using BackEndNotes.Services;
using BackEndNotes.Utils;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using MongoDB.Driver.Core.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add service database to Mongo
builder.Services.Configure<DatabaseModel>(builder.Configuration.GetSection("ConnectionStrings"));

// Add service of routes 
builder.Services.AddScoped<Context>();

builder.Services.AddScoped<IViews<UserModel>, UserCollection>();
builder.Services.AddScoped<UserService>();

builder.Services.AddScoped<IViewOne, MailCollection>();
builder.Services.AddScoped<MailService>();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
if (!app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("PermitirTodo");

app.UseAuthorization();

app.MapControllers();

app.Run();
