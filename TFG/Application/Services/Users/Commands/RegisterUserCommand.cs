using MediatR;

namespace TFG.Application.Services.Users.Commands;

public class RegisterUserCommand : IRequest
{
	public required string Email { get; set; }
	public required string Password { get; set; }
	public required string UserName { get; set; }
	public required string FirstName { get; set; }
	public required string LastName { get; set; }
	public string Language { get; set; } = "es";
}
