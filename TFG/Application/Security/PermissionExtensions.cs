using Microsoft.EntityFrameworkCore;
using Shared.Enums;
using TFG.Infrastructure.Data;

namespace TFG.Application.Security
{
	public static class PermissionExtensions
	{
		/// <summary>
		/// Obtiene y combina todos los permisos de los roles del usuario indicado.
		/// </summary>
		public static async Task<Permissions> GetCombinedPermissionsAsync(this ApplicationDbContext context, string userId)
		{
			var userPermissions = await context.Users.Include(u => u.Roles)
				.Where(u => u.Id == userId)
				.SelectMany(u => u.Roles.Select(r => r.Permissions))
				.ToListAsync();

			var test = (await context.Users.Include(u => u.Roles).ToListAsync());

			return userPermissions.Aggregate(Permissions.None, (acc, perm) => acc | perm);
		}
	}
}