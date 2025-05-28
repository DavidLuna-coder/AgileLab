using MediatR;
using Shared.DTOs.Roles;

namespace TFG.Application.Services.Roles.Queries.GetAllRoles
{
	public class GetAllRolesQuery : IRequest<IEnumerable<RolDto>>
	{

	}
}

