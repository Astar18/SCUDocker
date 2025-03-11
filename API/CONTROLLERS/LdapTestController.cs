using Microsoft.AspNetCore.Mvc;
using SCUDocker.APPLICATION.INTERFACES;

namespace SCUDocker.API.CONTROLLERS
{
    [Route("api/[controller]")]
    [ApiController]
    public class LdapTestController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        // Usamos la interfaz IUserRepository en lugar de UserRepository
        public LdapTestController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // Método POST para probar la conexión LDAP
        [HttpPost("TestConnection")]
        public IActionResult TestConnection([FromBody] LdapTestRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("El nombre de usuario y la contraseña son requeridos.");
            }

            // Usar el repositorio para probar la conexión LDAP con las credenciales
            bool isConnected = _userRepository.Authenticate(request.Username, request.Password);
            if (isConnected)
            {
                return Ok("Conexión LDAP exitosa");
            }
            else
            {
                return BadRequest("Error al conectar con el servidor LDAP");
            }
        }
    }

    // Clase de solicitud para los datos de prueba de conexión LDAP
    public class LdapTestRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
