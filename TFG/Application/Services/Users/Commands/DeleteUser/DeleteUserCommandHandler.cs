using MediatR;
using TFG.Api.Exeptions;
using TFG.Infrastructure.Data;

namespace TFG.Application.Services.Users.Commands.DeleteUser
{
	public class DeleteUserCommandHandler(ApplicationDbContext context) : IRequestHandler<DeleteUserCommand>
	{
		public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
		{
			var user = await context.Users.FindAsync(request.UserId)
				?? throw new NotFoundException("El usuario no existe");

			context.Users.Remove(user);

			await context.SaveChangesAsync(cancellationToken);
		}
	}
}
