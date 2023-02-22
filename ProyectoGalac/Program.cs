using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProyectoGalac;
using ProyectoGalac.Extensiones;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.RegisterDbContext();
builder.RegisterAppServices();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.ExecuteMigrations();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();



app.Run();
