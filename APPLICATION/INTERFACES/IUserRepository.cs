namespace SCUDocker.APPLICATION.INTERFACES
{
    public interface IUserRepository
    {
        /// <summary>
        /// Verifica si un usuario existe y sus credenciales son válidas en Active Directory.
        /// </summary>
        /// <param name="username">Nombre de usuario</param>
        /// <param name="password">Contraseña</param>
        /// <returns>True si la autenticación es exitosa, false si no.</returns>
        bool Authenticate(string username, string password);

        /// <summary>
        /// Obtiene el correo electrónico de un usuario en Active Directory.
        /// </summary>
        /// <param name="username">Nombre de usuario</param>
        /// <returns>Email del usuario.</returns>
        string GetUserEmail(string username);

        /// <summary>
        /// Obtiene la lista de roles de un usuario en Active Directory.
        /// </summary>
        /// <param name="username">Nombre de usuario</param>
        /// <returns>Lista de nombres de roles.</returns>
        List<string> GetUserRoles(string username);
    }
}
