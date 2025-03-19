
namespace TFG.SonarQubeClient.Impl
{
	public class SonarClient(IProjectsClient projects, IUsersManagementClient users, IDopTranslationsClient dopTranslations, IPermissionsClient permissions) : ISonarQubeClient
	{
		public IProjectsClient Projects { get; } = projects ?? throw new ArgumentNullException(nameof(projects));

		public IUsersManagementClient Users { get; } = users ?? throw new ArgumentNullException(nameof(users));

		public IDopTranslationsClient DopTranslations { get; } = dopTranslations ?? throw new ArgumentNullException(nameof(dopTranslations));

		public IPermissionsClient Permissions { get; } = permissions ?? throw new ArgumentNullException(nameof(permissions));
	}
}
