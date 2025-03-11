using System.Net;
using System.DirectoryServices.Protocols;
using SCUDocker.APPLICATION.INTERFACES;
namespace SCUDocker.INFRASTRUCTURE.REPOSITORIES
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _configuration;

        public UserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Método de autenticación de usuario con LDAP
        public bool Authenticate(string username, string password)
        {
            var ldapPath = _configuration["ActiveDirectory:LdapPath"];
            var ldapPort = int.Parse(_configuration["ActiveDirectory:LdapPort"] ?? "389");  // Puerto LDAP por defecto  

            try
            {
                using (var connection = new LdapConnection(new LdapDirectoryIdentifier(ldapPath, ldapPort)))
                {
                    connection.AuthType = AuthType.Basic;
                    connection.Bind(new NetworkCredential(username, password));  // Formato correcto para el bind con usuario y contraseña  

                    return true; // Si la conexión fue exitosa, el usuario está autenticado  
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error de autenticación en AD: {ex.Message}");
                return false;
            }
        }

        // Obtener el email de un usuario
        public string GetUserEmail(string username)
        {
            var ldapPath = _configuration["ActiveDirectory:LdapPath"];
            var ldapPort = int.Parse(_configuration["ActiveDirectory:LdapPort"] ?? "389");  // Puerto LDAP por defecto  
            var adminUser = _configuration["ActiveDirectory:Username"];
            var adminPassword = _configuration["ActiveDirectory:Password"];

            try
            {
                using (var connection = new LdapConnection(new LdapDirectoryIdentifier(ldapPath, ldapPort)))
                {
                    connection.AuthType = AuthType.Basic;
                    connection.Bind(new NetworkCredential(adminUser, adminPassword));  // Bind con un usuario administrador

                    var searchRequest = new SearchRequest(
                        "DC=epn,DC=local", // Base DN
                        $"(sAMAccountName={username})", // Filtro de búsqueda
                        SearchScope.Subtree,
                        new[] { "mail" } // Solo obtener el atributo de correo
                    );

                    var searchResponse = (SearchResponse)connection.SendRequest(searchRequest);

                    if (searchResponse.Entries.Count > 0)
                    {
                        return searchResponse.Entries[0].Attributes["mail"]?.ToString() ?? "No Email Found";
                    }

                    return "User Not Found";
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        // Obtener los roles (grupos) de un usuario
        public List<string> GetUserRoles(string username)
        {
            var ldapPath = _configuration["ActiveDirectory:LdapPath"];
            var ldapPort = int.Parse(_configuration["ActiveDirectory:LdapPort"] ?? "389");
            var adminUser = _configuration["ActiveDirectory:Username"];
            var adminPassword = _configuration["ActiveDirectory:Password"];
            var userRoles = new List<string>();

            try
            {
                using (var connection = new LdapConnection(new LdapDirectoryIdentifier(ldapPath, ldapPort)))
                {
                    connection.AuthType = AuthType.Basic;
                    connection.Bind(new NetworkCredential(adminUser, adminPassword)); // Usar usuario administrador para búsquedas

                    var searchRequest = new SearchRequest(
                        "DC=epn,DC=local", // Base DN de la búsqueda
                        $"(sAMAccountName={username})", // Filtro de búsqueda
                        SearchScope.Subtree,
                        new[] { "memberOf" } // Atributo de los grupos
                    );

                    var searchResponse = (SearchResponse)connection.SendRequest(searchRequest);

                    foreach (SearchResultEntry entry in searchResponse.Entries)
                    {
                        var memberOf = entry.Attributes["memberOf"];
                        if (memberOf != null)
                        {
                            foreach (var group in memberOf)
                            {
                                var groupName = group.ToString().Split(',')[0].Replace("CN=", "");
                                userRoles.Add(groupName);
                            }
                        }
                    }

                    return userRoles.Count > 0 ? userRoles : new List<string> { "No Roles Found" };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado al obtener roles: {ex.Message}");
                return new List<string> { "Error inesperado al obtener roles" };
            }
        }


        public bool TestLdapConnection()
        {
            var ldapPath = _configuration["ActiveDirectory:LdapPath"];
            var ldapPort = int.Parse(_configuration["ActiveDirectory:LdapPort"]);
            var username = _configuration["ActiveDirectory:Username"];
            var password = _configuration["ActiveDirectory:Password"];

            try
            {
                using (var connection = new LdapConnection(new LdapDirectoryIdentifier(ldapPath, ldapPort)))
                {
                    connection.Bind(new NetworkCredential(username, password)); // Realizar bind con las credenciales

                    return true;
                }
            }
            catch (LdapException ex)
            {
                Console.WriteLine($"Error al conectar con LDAP: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado: {ex.Message}");
                return false;
            }

            return false;
        }


    }
}
