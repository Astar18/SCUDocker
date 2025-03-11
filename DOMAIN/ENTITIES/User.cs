namespace SCUDocker.DOMAIN.ENTITIES
{
    public class User
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
    }
}
