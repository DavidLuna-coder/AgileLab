using MediatR;
using Shared.DTOs.Users;

namespace TFG.Application.Services.Users.Commands.EditUser
{
	public class EditUserCommand : IRequest<UserDto>
	{
		public string UserId { get; set; }
		public List<Guid> RolesIds { get; set; } = [];
	}
}
