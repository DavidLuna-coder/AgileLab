using Shared.Enums;
using TFG.Application.Security;
using TFG.Infrastructure.Data;

namespace TFG.Application.Services.Users.Commands.DeleteUser
{
	public class DeleteUserCommandPermission : IPermissionValidator<DeleteUserCommand>
	{
		private readonly IUserInfoAccessor _userInfoAccessor;
		private readonly ApplicationDbContext _context;
		public DeleteUserCommandPermission(IUserInfoAccessor userInfoAccessor, ApplicationDbContext context)
		{
			_userInfoAccessor = userInfoAccessor;
			_context = context;
		}
		public async Task<bool> HasPermissionAsync(DeleteUserCommand request)
		{
			var userId = _userInfoAccessor.UserInfo?.UserId ?? throw new Exception("UserId is null");
			var userPermissions = await _context.GetCombinedPermissionsAsync(userId);
			return (userPermissions & Permissions.DeleteUsers) != Permissions.None;
		}
	}
}
