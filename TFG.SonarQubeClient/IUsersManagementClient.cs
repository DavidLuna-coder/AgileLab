using TFG.SonarQubeClient.Models;

namespace TFG.SonarQubeClient
{
    public interface IUsersManagementClient
    {
        Task<User> CreateAsync(UserCreation user);
        Task DeleteAsync(string userId, bool anonymize = false);
    }
}
