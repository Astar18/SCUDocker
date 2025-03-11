namespace SCUDocker.APPLICATION.DTOs
{
    public class UserDto
    {
        public string Username { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public List<string> Roles { get; set; } = new List<string>();
    }
}
