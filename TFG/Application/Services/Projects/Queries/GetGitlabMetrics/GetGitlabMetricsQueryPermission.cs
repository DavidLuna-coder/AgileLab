using Shared.DTOs.Projects.Metrics;
using Shared.Enums;
using TFG.Application.Security;
using TFG.Infrastructure.Data;

namespace TFG.Application.Services.Projects.Queries.GetGitlabMetrics;

public class GetGitlabMetricsQueryPermission(IUserInfoAccessor userInfoAccessor, ApplicationDbContext context) : IPermissionValidator<GitlabMetricsDto>
{
	public async Task<bool> HasPermissionAsync(GitlabMetricsDto request)
	{
		var userId = userInfoAccessor.UserInfo?.UserId ?? throw new Exception("UserId is null");
		var userPermissions = await context.GetCombinedPermissionsAsync(userId);
		return (userPermissions & Permissions.ViewProjectKpis) != Permissions.None;
	}
}
