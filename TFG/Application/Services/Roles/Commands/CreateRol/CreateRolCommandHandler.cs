using MediatR;
using Shared.DTOs.Roles;
using TFG.Domain.Entities;
using TFG.Infrastructure.Data;

namespace TFG.Application.Services.Roles.Commands.CreateRol
{
	public class CreateRolCommandHandler(ApplicationDbContext dbContext) : IRequestHandler<CreateRolCommand, RolDto>
	{
		public async Task<RolDto> Handle(CreateRolCommand request, CancellationToken cancellationToken)
		{
			Rol newRol = new Rol
			{
				Name = request.Name,
				Permissions = request.Permissions
			};

			await dbContext.Roles.AddAsync(newRol, cancellationToken);
			await dbContext.AddAsync(cancellationToken, cancellationToken);

			RolDto createdRole = new RolDto
			{
				Name = newRol.Name,
				Permissions = newRol.Permissions
			};
			
			return createdRole;
		}
	}
}
