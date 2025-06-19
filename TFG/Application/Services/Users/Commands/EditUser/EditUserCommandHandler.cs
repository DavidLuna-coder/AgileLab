using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs.Users;
using TFG.Api.Exeptions;
using TFG.Api.Mappers;
using TFG.Domain.Entities;
using TFG.Infrastructure.Data;

namespace TFG.Application.Services.Users.Commands.EditUser
{
	public class EditUserCommandHandler(ApplicationDbContext context) : IRequestHandler<EditUserCommand, UserDto>
	{
		public async Task<UserDto> Handle(EditUserCommand request, CancellationToken cancellationToken)
		{
			User user = await context.Users.Where(u => u.Id == request.UserId).Include(u => u.Roles).FirstOrDefaultAsync() ?? throw new NotFoundException("User not found");
			user.Roles.Clear();
			List<Rol> roles = await context.Roles.Where(r => request.RolesIds.Contains(r.Id)).ToListAsync();
			
			user.Roles = roles;

			context.Users.Update(user);
			await context.SaveChangesAsync(cancellationToken);
			return user.ToUserDto();
		}
	}
}
