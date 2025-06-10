using Shared.Enums;
using TFG.Application.Security;
using TFG.Infrastructure.Data;

namespace TFG.Application.Services.Users.Commands
{
	public class RegisterUserCommandPermissions(IUserInfoAccessor userInfoAccessor, ApplicationDbContext context) : IPermissionValidator<RegisterUserCommand>
	{
		public async Task<bool> HasPermissionAsync(RegisterUserCommand request)
		{
			var userId = userInfoAccessor.UserInfo?.UserId ?? throw new Exception("UserId is null");
			var userPermissions = await context.GetCombinedPermissionsAsync(userId);
			return (userPermissions & Permissions.CreateUsers) != Permissions.None;
		}
	}
}
