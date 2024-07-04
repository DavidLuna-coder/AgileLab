namespace TFG.Model.Entities
{
    public class Rol
    {
        public Guid Id { get; set; }
        public ICollection<Permission> Permissions { get; set; }
    }
}
