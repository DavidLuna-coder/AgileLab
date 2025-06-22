using Microsoft.EntityFrameworkCore;
using Shared.Enums;
using TFG.Application.Security;
using TFG.Infrastructure.Data;

namespace TFG.Application.Services.Users.Queries.GetUserById
{
    public class GetUserByIdQueryPermissionValidator(IUserInfoAccessor userInfoAccessor, ApplicationDbContext context) : IPermissionValidator<GetUserByIdQuery>
    {
        private readonly Permissions requiredPermissions = Permissions.ViewUsers;
        public async Task<bool> HasPermissionAsync(GetUserByIdQuery request)
        {
            var userId = userInfoAccessor.UserInfo?.UserId ?? throw new Exception("UserId is null");
            var userPermissions = await context.Users
                .Where(u => u.Id == userId)
                .SelectMany(u => u.Roles.Select(r => r.Permissions))
                .ToListAsync();

            var combinedPermissions = userPermissions.Aggregate(Permissions.None, (acc, perm) => acc | perm);
            return (combinedPermissions & requiredPermissions) == requiredPermissions;
        }
    }
}
