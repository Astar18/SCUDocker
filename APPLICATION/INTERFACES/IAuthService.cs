using SCUDocker.APPLICATION.DTOs;

namespace SCUDocker.APPLICATION.INTERFACES
{
    public interface IAuthService
    {
        /// <summary>
        /// Autentica a un usuario en Active Directory.
        /// </summary>
        /// <param name="username">Nombre de usuario</param>
        /// <param name="password">Contraseña</param>
        /// <returns>Un `UserDto` si la autenticación es exitosa, `null` si falla.</returns>
        UserDto Authenticate(string username, string password);
    }

}
