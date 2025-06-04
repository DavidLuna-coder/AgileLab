using Shared.Enums;
using TFG.Application.Security;
using TFG.Infrastructure.Data;

namespace TFG.Application.Services.Roles.Commands.UpdateRol
{
	public class UpdateRolCommandPermissions(IUserInfoAccessor userInfoAccessor, ApplicationDbContext context) : IPermissionValidator<UpdateRolCommand>
	{
		public async Task<bool> HasPermissionAsync(UpdateRolCommand request)
		{
			var userInfo = userInfoAccessor.UserInfo 
				?? throw new UnauthorizedAccessException("User info is null");
			var combinedPermissions = await context.GetCombinedPermissionsAsync(userInfo.UserId);

			return (combinedPermissions & Permissions.EditRoles) != Permissions.None;
		}
	}
}
