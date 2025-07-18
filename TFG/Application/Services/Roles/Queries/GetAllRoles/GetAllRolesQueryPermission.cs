﻿using Shared.Enums;
using TFG.Application.Security;
using TFG.Infrastructure.Data;

namespace TFG.Application.Services.Roles.Queries.GetAllRoles
{
	public class GetAllRolesQueryPermission(IUserInfoAccessor userInfoAccessor, ApplicationDbContext context) : IPermissionValidator<GetAllRolesQuery>
	{
		public async Task<bool> HasPermissionAsync(GetAllRolesQuery request)
		{
			var userId = userInfoAccessor.UserInfo?.UserId ?? throw new Exception("UserId is null");
			var userPermissions = await context.GetCombinedPermissionsAsync(userId);
			return (userPermissions & Permissions.ViewRoles) != Permissions.None;
		}
	}
}

