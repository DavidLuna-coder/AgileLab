using MediatR;
using TFG.Infrastructure.Data;

namespace TFG.Application.Services.Roles.Commands.DeleteRol
{
	public class DeleteRolCommandHandler(ApplicationDbContext dbContext) : IRequestHandler<DeleteRolCommand>
	{
		public async Task Handle(DeleteRolCommand request, CancellationToken cancellationToken)
		{
			var rol = await dbContext.Roles.FindAsync(request.Id) 
				?? throw new KeyNotFoundException("El rol no existe");
			dbContext.Roles.Remove(rol);
			await dbContext.SaveChangesAsync(cancellationToken);
		}
	}
}
