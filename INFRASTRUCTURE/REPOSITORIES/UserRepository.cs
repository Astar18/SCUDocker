using System.Net;
using System.DirectoryServices.Protocols;
using SCUDocker.APPLICATION.INTERFACES;
using SCUDocker.DOMAIN.ENTITIES;
using System.DirectoryServices;
namespace SCUDocker.INFRASTRUCTURE.REPOSITORIES
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _ldapPath;
        private readonly string _adminUser;
        private readonly string _adminPassword;


        public UserRepository(string ldapPath, string adminUser, string adminPassword, IConfiguration configuration)
        {
            _configuration = configuration;
            _ldapPath = ldapPath;
            _adminUser = adminUser;
            _adminPassword = adminPassword;
        }


        //traer usuario
        public List<string> GetAllUsers()
        {
            List<string> users = new List<string>();

            try
            {
                string ldapPathT = $"LDAP://{_ldapPath}";
                using (DirectoryEntry dirEntry = new DirectoryEntry(ldapPathT, _adminUser, _adminPassword))
                {
                    using (DirectorySearcher searcher = new DirectorySearcher(dirEntry))
                    {
                        // Filtro para obtener todos los usuarios
                        searcher.Filter = "(&(objectClass=user)(objectCategory=person))";
                        searcher.SearchScope = System.DirectoryServices.SearchScope.Subtree;
                        searcher.PropertiesToLoad.Add("samAccountName"); // Cargar solo samAccountName

                        Console.WriteLine("Iniciando búsqueda...");
                        SearchResultCollection results = searcher.FindAll(); // Ejecutar búsqueda
                        Console.WriteLine($"Se encontraron {results.Count} resultados.");

                        foreach (SearchResult result in results)
                        {
                            if (result.Properties["samAccountName"] != null)
                            {
                                // Agregar nombre de usuario encontrado a la lista
                                string username = result.Properties["samAccountName"][0].ToString();
                                Console.WriteLine($"Usuario encontrado: {username}");
                                users.Add(username);
                            }
                        }
                    }
                }
            }
            catch (DirectoryServicesCOMException ex)
            {
                Console.WriteLine($"Error específico de LDAP: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener los usuarios: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }

            return users;
        }


        //crear usuario
        public void CreateUser(User user)
        {
            try
            {
                // Formato LDAP correcto para conexión
                string ldapPathT = $"LDAP://{_ldapPath}/CN=Users,DC=epn,DC=local";

                using (DirectoryEntry dirEntry = new DirectoryEntry(ldapPathT, _adminUser, _adminPassword))
                {
                    // Verificar si la ruta existe
                    try
                    {
                        object obj = dirEntry.NativeObject;  // Intentar acceder a la ruta
                        Console.WriteLine("La ruta 'CN=Users' es válida.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al acceder a 'CN=Users': {ex.Message}");
                        return;
                    }

                    // Crear el usuario en la ruta correcta
                    string userDn = $"CN={user.Username},CN=Users,DC=epn,DC=local";
                    using (DirectoryEntry newUser = dirEntry.Children.Add(userDn, "user"))
                    {
                        newUser.Properties["samAccountName"].Value = user.Username;
                        newUser.CommitChanges();

                        // Establecer la contraseña
                        newUser.Invoke("SetPassword", new object[] { user.Password });

                        // Activar la cuenta
                        newUser.Properties["userAccountControl"].Value = 0x200;  // Activa la cuenta
                        newUser.CommitChanges();
                    }
                }
            }
            catch (DirectoryServicesCOMException comEx)
            {
                Console.WriteLine($"Error específico de LDAP: {comEx.Message}");
                Console.WriteLine($"Stack trace: {comEx.StackTrace}");
            }
            catch (UnauthorizedAccessException authEx)
            {
                Console.WriteLine($"Error de autorización: {authEx.Message}");
                Console.WriteLine($"Stack trace: {authEx.StackTrace}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear el usuario: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
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



        public bool TestLdapConnection()
        {
            var ldapPath = _configuration["ActiveDirectory:LdapPath"];
            var username = _configuration["ActiveDirectory:Username"];
            var password = _configuration["ActiveDirectory:Password"];

            // Validar y convertir el puerto
            string ldapPortStr = _configuration["ActiveDirectory:LdapPort"];
            if (!int.TryParse(ldapPortStr, out int ldapPort))
            {
                Console.WriteLine($"Error: LdapPort '{ldapPortStr}' no es un número válido.");
                return false;
            }

            try
            {
                using (var connection = new LdapConnection(new LdapDirectoryIdentifier(ldapPath, ldapPort)))
                {
                    connection.Bind(new NetworkCredential(username, password)); // Realizar bind con las credenciales

                    Console.WriteLine("Conexión LDAP exitosa.");
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
        }



    }
}
