using SCUDocker.APPLICATION.INTERFACES;
using SCUDocker.INFRASTRUCTURE.CONFIGURATIONS;
using SCUDocker.INFRASTRUCTURE.REPOSITORIES;
using SCUDocker.SERVICES;

namespace SCUDocker.API
{
    public static class ServiceRegistration
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            // Configuración de Active Directory desde las variables de entorno
            services.Configure<ActiveDirectorySettings>(options =>
            {
                options.Domain = Environment.GetEnvironmentVariable("ACTIVEDIRECTORY__DOMAIN") ?? string.Empty;
                options.LdapPath = Environment.GetEnvironmentVariable("ACTIVEDIRECTORY__LDAPPATH") ?? string.Empty;
                options.LdapPort = int.Parse(Environment.GetEnvironmentVariable("ACTIVEDIRECTORY__LDAPPORT") ?? "0");
                options.Username = Environment.GetEnvironmentVariable("ACTIVEDIRECTORY__USERNAME") ?? string.Empty;
                options.Password = Environment.GetEnvironmentVariable("ACTIVEDIRECTORY__PASSWORD") ?? string.Empty;
            });

            // Inyección de dependencias
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
        }
    }
}
