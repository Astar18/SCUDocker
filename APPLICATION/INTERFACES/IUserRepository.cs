using SCUDocker.DOMAIN.ENTITIES;
namespace SCUDocker.APPLICATION.INTERFACES
{
    public interface IUserRepository
    {
             bool Authenticate(string username, string password);

        
            void CreateUser(User user);
            List<string> GetAllUsers();
        
    }
}
