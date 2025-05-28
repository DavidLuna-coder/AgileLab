using MediatR;
using Shared.DTOs.Roles;
using Shared.Enums;

namespace TFG.Application.Services.Roles.Commands.CreateRol
{
	public class CreateRolCommand : IRequest<RolDto>
	{
		public string Name { get; set; }
		public Permissions Permissions { get; set; }
	}
}
