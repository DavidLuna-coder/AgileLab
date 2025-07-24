using Shared.Enums;
using TFG.Application.Security;
using TFG.Infrastructure.Data;

namespace TFG.Application.Services.Experiences.Commands;

    public class CreateGoRaceExperienceCommandPermission(IUserInfoAccessor userInfoAccessor, ApplicationDbContext context) : IPermissionValidator<CreateGoRaceExperienceCommand>
    {
        public async Task<bool> HasPermissionAsync(CreateGoRaceExperienceCommand request)
        {
            var userId = userInfoAccessor.UserInfo?.UserId ?? throw new Exception("UserId is null");
            var userPermissions = await context.GetCombinedPermissionsAsync(userId);
            return (userPermissions & Permissions.CreateExperiences) != Permissions.None;
        }
    }

    public class UpdateGoRaceExperienceCommandPermission(IUserInfoAccessor userInfoAccessor, ApplicationDbContext context) : IPermissionValidator<UpdateGoRaceExperienceCommand>
    {
        public async Task<bool> HasPermissionAsync(UpdateGoRaceExperienceCommand request)
        {
            var userId = userInfoAccessor.UserInfo?.UserId ?? throw new Exception("UserId is null");
            var userPermissions = await context.GetCombinedPermissionsAsync(userId);
            return (userPermissions & Permissions.EditExperiences) != Permissions.None;
        }
    }

    public class DeleteGoRaceExperienceCommandPermission(IUserInfoAccessor userInfoAccessor, ApplicationDbContext context) : IPermissionValidator<DeleteGoRaceExperienceCommand>
    {
        public async Task<bool> HasPermissionAsync(DeleteGoRaceExperienceCommand request)
        {
            var userId = userInfoAccessor.UserInfo?.UserId ?? throw new Exception("UserId is null");
            var userPermissions = await context.GetCombinedPermissionsAsync(userId);
            return (userPermissions & Permissions.DeleteExperiences) != Permissions.None;
        }
    }
