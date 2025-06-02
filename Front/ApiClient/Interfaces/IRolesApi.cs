using Shared.DTOs.Roles;

namespace Front.ApiClient.Interfaces
{
	public interface IRolesApi
	{
		Task CreateRol(CreateRolDto createRolDto);
		Task<IEnumerable<RolDto>> GetRoles();
		Task<RolDto> GetRol(Guid id);
	}
}
