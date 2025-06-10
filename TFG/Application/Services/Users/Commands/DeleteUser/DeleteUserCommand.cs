using MediatR;
using Microsoft.EntityFrameworkCore;

namespace TFG.Application.Services.Users.Commands.DeleteUser
{
	public class DeleteUserCommand : IRequest
	{
		public string UserId { get; set; }
	}
}
