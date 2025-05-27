namespace TFG.Domain.Entities
{
    public class Permission
    {
        public Guid Id { get; set; }
        public ICollection<Rol> Roles { get; set;}
    }
}
