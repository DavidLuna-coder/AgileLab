using Shared.Enums;
using TFG.Application.Security;
using TFG.Infrastructure.Data;

namespace TFG.Application.Services.Projects.Commands.CreateProject;

public class CreateProjectCommandPermission(IUserInfoAccessor userInfoAccessor, ApplicationDbContext context) : IPermissionValidator<CreateProjectCommand>
{
	public async Task<bool> HasPermissionAsync(CreateProjectCommand request)
	{
		var userId = userInfoAccessor.UserInfo?.UserId ?? throw new Exception("UserId is null");
		var userPermissions = await context.GetCombinedPermissionsAsync(userId);
		return (userPermissions & Permissions.CreateProjects) != Permissions.None;
	}
}