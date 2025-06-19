namespace Shared.DTOs.Users;

public class EditUserDto
{
	public required List<Guid> RolesIds { get; set; } = [];
}
