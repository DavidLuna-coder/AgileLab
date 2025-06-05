using Shared.Enums;
using TFG.Application.Security;
using TFG.Infrastructure.Data;

namespace TFG.Application.Services.Roles.Commands.DeleteRol
{
	public class DeleteRolCommandPermissions(IUserInfoAccessor userInfoAccessor, ApplicationDbContext context) : IPermissionValidator<DeleteRolCommand>
	{
		public async Task<bool> HasPermissionAsync(DeleteRolCommand request)
		{
			var userInfo = userInfoAccessor.UserInfo
				?? throw new UnauthorizedAccessException("User info is null");
			var combinedPermissions = await context.GetCombinedPermissionsAsync(userInfo.UserId);

			return (combinedPermissions & Permissions.DeleteRoles) != Permissions.None;
		}
	}
}
