using MediatR;
using Shared.DTOs.Roles;

namespace TFG.Application.Services.Roles.Commands.UpdateRol
{
	public class UpdateRolCommand : IRequest<RolDto>
	{
		public Guid Id { get; set; }
		public UpdateRolDto UpdateRol { get; set; }
	}
}
