using SCUDocker.APPLICATION.DTOs;
using SCUDocker.APPLICATION.INTERFACES;

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
                Email = _userRepository.GetUserEmail(username),
                Roles = _userRepository.GetUserRoles(username)
            };
        }

        public List<UserDto> GetUsersByGroup(string groupName)
        {
            // Implementación para obtener usuarios por grupo en Active Directory
            throw new NotImplementedException();
        }
    }
}
