using Shared.Enums;
using TFG.Application.Security;
using TFG.Infrastructure.Data;

namespace TFG.Application.Services.Roles.Queries.GetRol
{
	public class GetRolQueryPermission : IPermissionValidator<GetRolQuery>
	{
		private readonly IUserInfoAccessor _userInfoAccessor;
		private readonly ApplicationDbContext _context;
		public GetRolQueryPermission(IUserInfoAccessor userInfoAccessor, ApplicationDbContext context)
		{
			_userInfoAccessor = userInfoAccessor;
			_context = context;
		}
		public async Task<bool> HasPermissionAsync(GetRolQuery request)
		{
			var userId = _userInfoAccessor.UserInfo?.UserId ?? throw new Exception("UserId is null");
			var userPermissions = await _context.GetCombinedPermissionsAsync(userId);
			return (userPermissions & Permissions.ViewRoles) != Permissions.None;
		}
	}
}
