using Shared.Enums;
using TFG.Application.Security;
using TFG.Infrastructure.Data;

namespace TFG.Application.Services.Projects.Commands.DeleteProject
{
	public class DeleteProjectCommandPermissions(IUserInfoAccessor userInfoAccessor, ApplicationDbContext context) : IPermissionValidator<DeleteProjectCommand>
	{
		public async Task<bool> HasPermissionAsync(DeleteProjectCommand request)
		{
			var userId = userInfoAccessor.UserInfo?.UserId ?? throw new InvalidOperationException("UserId is null");
			var userPermissions = await context.GetCombinedPermissionsAsync(userId);
			return (userPermissions & Permissions.DeleteProjects) != Permissions.None;
		}
	}
}
