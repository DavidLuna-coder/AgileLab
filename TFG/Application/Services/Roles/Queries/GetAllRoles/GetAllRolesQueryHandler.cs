using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs.Roles;
using TFG.Api.Mappers;
using TFG.Infrastructure.Data;

namespace TFG.Application.Services.Roles.Queries.GetAllRoles
{
	public class GetAllRolesQueryHandler(ApplicationDbContext context) : IRequestHandler<GetAllRolesQuery, IEnumerable<RolDto>>
	{
		public async Task<IEnumerable<RolDto>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
		{
			var roles = await context.Roles.ToListAsync(cancellationToken);
			return roles.Select(rol => rol.ToRolDto()).ToList();
		}
	}
}

