using Shared.Enums;
using TFG.Application.Security;
using TFG.Infrastructure.Data;

namespace TFG.Application.Services.Projects.Commands.ArchiveProject
{
    public class ArchiveProjectCommandPermission(IUserInfoAccessor userInfoAccessor, ApplicationDbContext context) : IPermissionValidator<ArchiveProjectCommand>
    {
        public async Task<bool> HasPermissionAsync(ArchiveProjectCommand request)
        {
            var userId = userInfoAccessor.UserInfo?.UserId ?? throw new InvalidOperationException("UserId is null");
            var userPermissions = await context.GetCombinedPermissionsAsync(userId);
            return (userPermissions & Permissions.ArchiveProjects) != Permissions.None;
        }
    }
}
