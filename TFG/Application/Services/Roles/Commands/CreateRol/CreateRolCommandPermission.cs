using Shared.Enums;
using TFG.Application.Security;
using TFG.Infrastructure.Data;

namespace TFG.Application.Services.Roles.Commands.CreateRol
{
	public class CreateRolCommandPermission(IUserInfoAccessor userInfoAccesor, ApplicationDbContext context) : IPermissionValidator<CreateRolCommand>
	{
		public async Task<bool> HasPermissionAsync(CreateRolCommand request)
		{
			var userId = userInfoAccesor.UserInfo?.UserId ?? throw new Exception("UserId is null");

			var combinedPermissions = await context.GetCombinedPermissionsAsync(userId);

			return (combinedPermissions & Permissions.CreateRoles) != Permissions.None;
		}
	}
}
