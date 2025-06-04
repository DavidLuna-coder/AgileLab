using Front.ApiClient.Interfaces;
using Shared.DTOs.Roles;

namespace Front.ApiClient.Implementations
{
	public class RolesApi(IApiHttpClient client) : IRolesApi
	{

		public async Task CreateRol(CreateRolDto createRolDto)
		{
			await client.PostAsync<CreateRolDto, object>("api/roles", createRolDto);
		}

		public async Task<IEnumerable<RolDto>> GetRoles()
		{
			var response = await client.GetAsync<IEnumerable<RolDto>>("api/roles");
			return response;
		}
		public async Task<RolDto> GetRol(Guid id)
		{
			var response = await client.GetAsync<RolDto>($"api/roles/{id}");
			return response;
		}

		public async Task<RolDto> UpdateRol(Guid id, UpdateRolDto updateRolDto)
		{
			var response = await client.PutAsync<UpdateRolDto, RolDto>($"api/roles/{id}", updateRolDto);
			return response;
		}
	}
}
