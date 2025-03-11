using DotNetEnv;
using SCUDocker.API;
using SCUDocker.INFRASTRUCTURE;

using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Cargar las variables de entorno desde el archivo .env
Env.Load();

builder.Services.AddApplicationServices();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.ShowExtensions();
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "SCUDocker API v1");
    options.RoutePrefix = string.Empty; 
});

app.UseAuthorization();
app.MapControllers();

app.Run();
