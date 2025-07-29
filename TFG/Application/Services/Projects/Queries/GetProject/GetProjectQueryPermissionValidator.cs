using Microsoft.EntityFrameworkCore;
using Shared.Enums;
using TFG.Application.Security;
using TFG.Infrastructure.Data;

namespace TFG.Application.Services.Projects.Queries.GetProject
{
    public class GetProjectQueryPermissionValidator(IUserInfoAccessor userInfoAccessor, ApplicationDbContext context) : IPermissionValidator<GetProjectQuery>
    {
        public async Task<bool> HasPermissionAsync(GetProjectQuery request)
        {
            var userId = userInfoAccessor.UserInfo?.UserId ?? request.UserId;
            if (userId == null)
                return false;

            var userPermissions = await context.GetCombinedPermissionsAsync(userId);
            if ((userPermissions & Permissions.ViewAllProjects) != Permissions.None)
                return true;

            // Si no tiene permiso global, comprobar si pertenece al proyecto
            var isMember = await context.Projects
                .Where(p => p.Id == request.ProjectId)
                .AnyAsync(p => p.Users.Any(u => u.Id == userId));
            return isMember;
        }
    }
}
