using Shared.Enums;

namespace Shared.DTOs.Roles
{
	public class CreateRolDto 	
	{
		public required string Name { get; set; }
		public Permissions Permissions { get; set; } 
	}
}
