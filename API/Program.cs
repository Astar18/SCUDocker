using DotNetEnv;
using SCUDocker.API;
using SCUDocker.INFRASTRUCTURE;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Cargar las variables de entorno desde el archivo .env (forzando la ruta correcta)
string envPath = Path.Combine(Directory.GetCurrentDirectory(), ".env");
Env.Load(envPath);
Console.WriteLine($"Cargando variables desde: {envPath}");

// Imprimir las variables cargadas para verificar
Console.WriteLine($"ACTIVEDIRECTORY__DOMAIN = {Environment.GetEnvironmentVariable("ACTIVEDIRECTORY__DOMAIN")}");
Console.WriteLine($"ACTIVEDIRECTORY__LDAPPORT = {Environment.GetEnvironmentVariable("ACTIVEDIRECTORY__LDAPPORT")}");

// Reemplazar valores en la configuración con variables de entorno
foreach (var key in Environment.GetEnvironmentVariables().Keys)
{
    string? keyString = key.ToString();
    if (!string.IsNullOrEmpty(keyString))
    {
        var value = Environment.GetEnvironmentVariable(keyString);
        string normalizedKey = keyString.Replace("__", ":");
        builder.Configuration[normalizedKey] = value;
    }
}

// Registrar los servicios y pasar la configuración
builder.Services.AddApplicationServices(builder.Configuration);

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

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
