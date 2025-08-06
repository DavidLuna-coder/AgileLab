using Shared.Enums;
using TFG.Application.Security;
using TFG.Infrastructure.Data;

namespace TFG.Application.Services.Projects.Queries.GetOpenProjectMetrics;

public class GetOpenProjectMetricsQueryPermissions(IUserInfoAccessor userInfoAccessor, ApplicationDbContext context) : IPermissionValidator<GetOpenProjectMetricsQuery>
{
	public async Task<bool> HasPermissionAsync(GetOpenProjectMetricsQuery request)
	{
		var userId = userInfoAccessor.UserInfo?.UserId ?? throw new Exception("UserId is null");
		var userPermissions = await context.GetCombinedPermissionsAsync(userId);
		return (userPermissions & Permissions.ViewProjectKpis) != Permissions.None;
	}
}
