namespace SCUDocker.DOMAIN.ENTITIES
{
    public class Roles
    {
        public int Id { get; set; }      // Identificador único del rol
        public string Name { get; set; } = string.Empty;  // Nombre del rol
        public string Description { get; set; } = string.Empty; // Descripción del rol

        // Relación con usuarios si aplica (descomenta si usas una relación con usuarios)
        // public ICollection<User> Users { get; set; } = new List<User>();
    }
}
