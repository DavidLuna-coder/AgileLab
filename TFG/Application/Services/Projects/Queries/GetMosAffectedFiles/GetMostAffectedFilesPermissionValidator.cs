using Microsoft.EntityFrameworkCore;
using Shared.Enums;
using TFG.Application.Security;
using TFG.Infrastructure.Data;

namespace TFG.Application.Services.Projects.Queries.GetMosAffectedFiles
{
	public class GetMostAffectedFilesPermissionValidator(IUserInfoAccessor userInfoAccessor, ApplicationDbContext context) : IPermissionValidator<GetMostAffectedFilesQuery>
	{
		private Permissions requiredPermissions = Permissions.ViewProjectKpis;
		public async Task<bool> HasPermissionAsync(GetMostAffectedFilesQuery request)
		{
			var userId = userInfoAccessor.UserInfo?.UserId ?? throw new Exception("UserId is null");
			var userPermissions = await context.Users
				.Where(u => u.Id == userId)
				.SelectMany(u => u.Roles.Select(r => r.Permissions))
				.ToListAsync();

			// Combina los permisos usando OR bitwise
			var combinedPermissions = userPermissions.Aggregate(Permissions.None, (acc, perm) => acc | perm);

			// Comprueba si el usuario tiene el permiso requerido
			return (combinedPermissions & requiredPermissions) == requiredPermissions;
		}
	}
}
