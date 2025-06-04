using Shared.Enums;

namespace Shared.DTOs.Roles
{
	public class UpdateRolDto
	{
		public string Name { get; set; }
		public Permissions Permissions { get; set; }
	}
}
