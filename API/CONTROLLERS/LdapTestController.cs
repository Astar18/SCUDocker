using Microsoft.AspNetCore.Mvc;
using SCUDocker.APPLICATION.DTOs;
using SCUDocker.APPLICATION.INTERFACES;
using SCUDocker.SERVICES;

namespace SCUDocker.API.CONTROLLERS
{
    [Route("api/[controller]")]
    [ApiController]
    public class LdapTestController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;


        // Usamos la interfaz IUserRepository en lugar de UserRepository
        public LdapTestController(IUserRepository userRepository,IUserService  userService)
        {
            _userRepository = userRepository;
            _userService = userService;
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
        [HttpPost("register")]
        public IActionResult Register(UserDto userDto)
        {
            _userService.RegisterUser(userDto);
            return Ok("Usuario registrado exitosamente.");
        }
        [HttpGet("all-users")]
        public ActionResult<List<string>> GetAllUsers()
        {
            var users = _userService.GetAllUsers();
            if (users == null || users.Count == 0)
            {
                return NotFound("No se encontraron usuarios.");
            }
            return Ok(users);
        }
        [HttpGet("profiles")]
        public IActionResult GetUserProfiles()
        {
            var profiles = _userService.GetUserProfiles();
            if (profiles.Count == 0)
            {
                return NotFound("No se encontraron perfiles de usuario.");
            }
            return Ok(profiles);
        }


        
    }
    

    // Clase de solicitud para los datos de prueba de conexión LDAP
    public class LdapTestRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
