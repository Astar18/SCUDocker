using Microsoft.AspNetCore.Mvc;
using SCUDocker.INFRASTRUCTURE.SERVICES;

namespace SCUDocker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LdapController : ControllerBase
    {
        private readonly LdapService _ldapService;
        private readonly IConfiguration _configuration;


        public LdapController(LdapService ldapService,IConfiguration configuration)
        {
            _ldapService = ldapService;
            _configuration = configuration;
        }

        [HttpGet("domains")]
public IActionResult GetDomains()
{
    try
    {
        // Obtener las configuraciones desde el archivo .env cargado en IConfiguration
        string ldapServer = _configuration["ActiveDirectory:LdapPath"] ?? string.Empty;
        string adminUser = _configuration["ActiveDirectory:Username"] ?? string.Empty;
        string adminPassword = _configuration["ActiveDirectory:Password"] ?? string.Empty;

        // Verificar si los valores están presentes
        if (string.IsNullOrEmpty(ldapServer) || string.IsNullOrEmpty(adminUser) || string.IsNullOrEmpty(adminPassword))
        {
            return BadRequest("La configuración de Active Directory no está correctamente configurada.");
        }

        // Llamar al servicio LDAP para obtener los dominios
        _ldapService.GetDomains(ldapServer, adminUser, adminPassword); 

        return Ok("Consulta de dominios realizada con éxito.");
    }
    catch (Exception ex)
    {
        return StatusCode(500, $"Error al obtener los dominios: {ex.Message}");
    }
}


        // Nuevo método GET para obtener todos los grupos
        [HttpGet("groups")]
        public IActionResult GetAllGroups()
        {
            try
            {
                // Obtener las configuraciones desde el archivo .env cargado en IConfiguration
                string ldapServer = _configuration["ActiveDirectory:LdapPath"];
                string adminUser = _configuration["ActiveDirectory:Username"];
                string adminPassword = _configuration["ActiveDirectory:Password"];

                // Verificar si los valores están presentes
                if (string.IsNullOrEmpty(ldapServer) || string.IsNullOrEmpty(adminUser) || string.IsNullOrEmpty(adminPassword))
                {
                    return BadRequest("La configuración de Active Directory no está correctamente configurada.");
                }

                // Llamar al servicio LDAP para obtener los grupos
                var groups = _ldapService.GetAllGroupsList(ldapServer, adminUser, adminPassword);

                // Verificar si se encontraron grupos
                if (groups == null || groups.Count == 0)
                {
                    return NotFound("No se encontraron grupos.");
                }

                // Devolver la lista de grupos encontrados
                return Ok(groups);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener los grupos: {ex.Message}");
            }
        }











    }
}
