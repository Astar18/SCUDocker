using Microsoft.AspNetCore.Mvc;
using SCUDocker.APPLICATION.DTOs;
using SCUDocker.APPLICATION.INTERFACES;

namespace SCUDocker.API.CONTROLLERS
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthenticationController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestDto request)
        {
            var result = _authService.Authenticate(request.Username, request.Password);
            if (result == null) return Unauthorized();
            return Ok(result);
        }
        
    }
}
