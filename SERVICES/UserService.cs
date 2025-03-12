using SCUDocker.APPLICATION.DTOs;
using SCUDocker.APPLICATION.INTERFACES;
using SCUDocker.DOMAIN.ENTITIES;
using System.Runtime.InteropServices;
using System.DirectoryServices;

namespace SCUDocker.SERVICES
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly string _ldapServer;
        private readonly string _adminUser;
        private readonly string _adminPassword;


        public UserService(IUserRepository userRepository,string ldapServer, string adminUser, string adminPassword)
        {
            _userRepository = userRepository;
            _ldapServer = ldapServer;
            _adminUser = adminUser;
            _adminPassword = adminPassword;
            
        }

        public UserDto GetUserDetails(string username)
        {
            return new UserDto
            {
                Username = username,
                
            };
        }

        public List<UserDto> GetUsersByGroup(string groupName)
        {
            // Implementación para obtener usuarios por grupo en Active Directory
            throw new NotImplementedException();
        }
        public void RegisterUser(UserDto userDto)
        {
            var user = new User
            {
                Username = userDto.Username,
                Password = userDto.Password
                
            };

            _userRepository.CreateUser(user);
        }
        public List<string> GetAllUsers()
        {
            return _userRepository.GetAllUsers();
        }



        //obtener perfiles de usuario
        public List<Dictionary<string, object>> GetUserProfiles()
    {
        List<Dictionary<string, object>> usersList = new List<Dictionary<string, object>>();

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            try
            {
                string ldapPath = $"LDAP://{_ldapServer}";
                using (DirectoryEntry entry = new DirectoryEntry(ldapPath, _adminUser, _adminPassword))
                {
                    using (DirectorySearcher searcher = new DirectorySearcher(entry))
                    {
                        searcher.Filter = "(objectClass=user)";
                        searcher.SearchScope = SearchScope.Subtree;
                        searcher.PropertiesToLoad.Add("cn");
                        searcher.PropertiesToLoad.Add("samAccountName");
                        searcher.PropertiesToLoad.Add("mail");
                        searcher.PropertiesToLoad.Add("memberOf");

                        SearchResultCollection results = searcher.FindAll();

                        foreach (SearchResult result in results)
                        {
                            Dictionary<string, object> userProfile = new Dictionary<string, object>();

                            foreach (string propertyName in result.Properties.PropertyNames)
                            {
                                ResultPropertyValueCollection values = result.Properties[propertyName];
                                if (values.Count > 1)
                                {
                                    userProfile[propertyName] = values.Cast<object>().ToList();
                                }
                                else
                                {
                                    userProfile[propertyName] = values[0]?.ToString();
                                }
                            }
                            usersList.Add(userProfile);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener los perfiles de usuario: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("LDAP functionality is only supported on Windows.");
        }

        return usersList;
    }


    }
}
