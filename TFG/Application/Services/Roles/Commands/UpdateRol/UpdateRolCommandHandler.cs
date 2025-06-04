using MediatR;
using Shared.DTOs.Roles;
using TFG.Api.Exeptions;
using TFG.Api.Mappers;
using TFG.Domain.Entities;
using TFG.Infrastructure.Data;

namespace TFG.Application.Services.Roles.Commands.UpdateRol
{
	public class UpdateRolCommandHandler(ApplicationDbContext dbContext) : IRequestHandler<UpdateRolCommand, RolDto>
	{
		public async Task<RolDto> Handle(UpdateRolCommand request, CancellationToken cancellationToken)
		{
			Rol rol = await dbContext.Roles.FindAsync(request.Id) 
								?? throw new NotFoundException("El rol no existe");

			rol.Name = request.UpdateRol.Name;
			rol.Permissions = request.UpdateRol.Permissions;
			dbContext.Roles.Update(rol);
			await dbContext.SaveChangesAsync();

			return rol.ToRolDto();
		}
	}
}
