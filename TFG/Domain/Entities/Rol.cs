using Shared.Enums;

namespace TFG.Domain.Entities
{
    public class Rol
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // Almacena los permisos como un valor entero (bitmask)
        public Permissions Permissions { get; set; }

        // Relación con usuarios (opcional)
        public ICollection<User> Users { get; set; }
    }
}