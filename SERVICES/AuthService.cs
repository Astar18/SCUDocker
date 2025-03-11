using SCUDocker.APPLICATION.DTOs;
using SCUDocker.APPLICATION.INTERFACES;

namespace SCUDocker.SERVICES
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public UserDto Authenticate(string username, string password)
        {
            bool isValidUser = _userRepository.Authenticate(username, password);
            if (!isValidUser) return null;

            return new UserDto
            {
                Username = username,
                
            };
        }
    }
}
