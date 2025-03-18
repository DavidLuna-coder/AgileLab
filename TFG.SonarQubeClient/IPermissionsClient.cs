using TFG.SonarQubeClient.Models;

namespace TFG.SonarQubeClient
{
    public interface IPermissionsClient
    {
        Task AddUserAsync(UserPermission userPermission);
    }
}
