using SCUDocker.APPLICATION.INTERFACES;
using SCUDocker.INFRASTRUCTURE.REPOSITORIES;
using SCUDocker.SERVICES;
using SCUDocker.INFRASTRUCTURE.CONFIGURATIONS;
using Microsoft.Extensions.Configuration;
using SCUDocker.INFRASTRUCTURE.SERVICES;

namespace SCUDocker.API
{
    public static class ServiceRegistration
    {
        public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Registrar IUserRepository con valores de configuración leídos de las variables de entorno
            services.AddScoped<IUserRepository>(provider =>
            {
                // Obtener los valores de configuración directamente
                string ldapPath = configuration["ActiveDirectory:LdapPath"];
                string adminUser = configuration["ActiveDirectory:Username"];
                string adminPassword = configuration["ActiveDirectory:Password"];

                if (string.IsNullOrEmpty(ldapPath) || string.IsNullOrEmpty(adminUser) || string.IsNullOrEmpty(adminPassword))
                {
                    throw new InvalidOperationException("La configuración de Active Directory no está correctamente configurada.");
                }

                // Crear UserRepository con los valores obtenidos y pasar IConfiguration
                return new UserRepository(ldapPath, adminUser, adminPassword, configuration);
            });

            // Asegúrate de que UserService esté registrado correctamente con sus dependencias
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<LdapService>();

            // Otros servicios
            services.AddScoped<IAuthService, AuthService>();
        }
    }
}
