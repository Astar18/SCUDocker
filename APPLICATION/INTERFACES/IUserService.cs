using SCUDocker.APPLICATION.DTOs;

namespace SCUDocker.APPLICATION.INTERFACES
{
    public interface IUserService
    {
        
        UserDto GetUserDetails(string username);

        void RegisterUser(UserDto userDto);
        List<UserDto> GetUsersByGroup(string groupName);
        List<string> GetAllUsers();
    }
}
