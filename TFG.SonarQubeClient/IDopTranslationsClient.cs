using TFG.SonarQubeClient.Models;

namespace TFG.SonarQubeClient
{
    public interface IDopTranslationsClient
    {
        Task<DopSettingsResponse> GetDopSettingsAsync();
        Task<BoundedProject> BoundProjectAsync(ProjectBinding projectBinding);
    }
}
