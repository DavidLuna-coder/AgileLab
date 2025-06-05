using MediatR;

namespace TFG.Application.Services.Roles.Commands.DeleteRol
{
	public class DeleteRolCommand : IRequest
	{
		public Guid Id { get; set; }
	}
}
