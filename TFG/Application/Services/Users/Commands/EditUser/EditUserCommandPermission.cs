using Shared.Enums;
using TFG.Application.Security;
using TFG.Infrastructure.Data;

namespace TFG.Application.Services.Users.Commands.EditUser
{
	public class EditUserCommandPermission : IPermissionValidator<EditUserCommand>
	{
		private readonly IUserInfoAccessor _userInfoAccessor;
		private readonly ApplicationDbContext _context;
		public EditUserCommandPermission(IUserInfoAccessor userInfoAccessor, ApplicationDbContext context)
		{
			_userInfoAccessor = userInfoAccessor;
			_context = context;
		}
		public async Task<bool> HasPermissionAsync(EditUserCommand request)
		{
			var userId = _userInfoAccessor.UserInfo?.UserId ?? throw new Exception("UserId is null");
			var userPermissions = await _context.GetCombinedPermissionsAsync(userId);
			return (userPermissions & Permissions.EditUsers) != Permissions.None;
		}
	}
}
