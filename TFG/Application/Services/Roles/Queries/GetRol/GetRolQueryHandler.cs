using MediatR;
using Shared.DTOs.Roles;
using TFG.Api.Exeptions;
using TFG.Infrastructure.Data;

namespace TFG.Application.Services.Roles.Queries.GetRol
{
	public class GetRolQueryHandler(ApplicationDbContext context) : IRequestHandler<GetRolQuery, RolDto>
	{
		public async Task<RolDto> Handle(GetRolQuery request, CancellationToken cancellationToken)
		{
			var rol = await context.Roles.FindAsync(request.Id) ?? throw new NotFoundException($"Role with ID {request.Id} not found.");
			return new RolDto
			{
				Id = rol.Id,
				Name = rol.Name,
				Permissions = rol.Permissions
			};
		}
	}
}
