using Shared.Enums;

namespace Shared.DTOs.Roles
{
	public class RolDto
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public Permissions Permissions { get; set; }
	}
}
