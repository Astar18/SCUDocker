namespace SCUDocker.INFRASTRUCTURE.CONFIGURATIONS
{
    public class ActiveDirectorySettings
    {
        public required string Domain { get; set; }
        public required string LdapPath { get; set; }
        public required int LdapPort { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
