using SCUDocker.APPLICATION.DTOs;

namespace SCUDocker.APPLICATION.INTERFACES
{
    public interface IUserService
    {
        /// <summary>
        /// Obtiene los detalles de un usuario en Active Directory.
        /// </summary>
        /// <param name="username">Nombre de usuario</param>
        /// <returns>Un `UserDto` con los datos del usuario.</returns>
        UserDto GetUserDetails(string username);

        /// <summary>
        /// Obtiene una lista de usuarios de un grupo específico en AD.
        /// </summary>
        /// <param name="groupName">Nombre del grupo</param>
        /// <returns>Lista de `UserDto` con los datos de los usuarios.</returns>
        List<UserDto> GetUsersByGroup(string groupName);
    }
}
