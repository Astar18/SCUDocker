using SCUDocker.APPLICATION.DTOs;
using SCUDocker.APPLICATION.INTERFACES;
using SCUDocker.DOMAIN.ENTITIES;

namespace SCUDocker.SERVICES
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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


    }
}
