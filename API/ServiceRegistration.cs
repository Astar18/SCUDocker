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
        // Obtener los valores de configuración directamente
        string ldapServer = configuration["ActiveDirectory:LdapPath"] ?? throw new InvalidOperationException("LdapPath no está configurado.");
        string adminUser = configuration["ActiveDirectory:Username"] ?? throw new InvalidOperationException("Username no está configurado.");
        string adminPassword = configuration["ActiveDirectory:Password"] ?? throw new InvalidOperationException("Password no está configurado.");

        // Registrar IUserRepository con las credenciales del .env
        services.AddScoped<IUserRepository>(provider =>
            new UserRepository(ldapServer, adminUser, adminPassword, configuration));

        // Registrar IUserService correctamente con sus dependencias
        services.AddScoped<IUserService>(provider =>
            new UserService(
                provider.GetRequiredService<IUserRepository>(),
                ldapServer,
                adminUser,
                adminPassword
            ));

        // Registrar otros servicios
        services.AddScoped<LdapService>();
        services.AddScoped<IAuthService, AuthService>();
    }

    }
}
