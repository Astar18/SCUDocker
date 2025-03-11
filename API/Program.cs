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

// Configuraci�n del pipeline HTTP
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
    options.RoutePrefix = string.Empty; // Esto hace que se abra Swagger autom�ticamente en la ra�z
});

app.UseAuthorization();
app.MapControllers();

app.Run();
