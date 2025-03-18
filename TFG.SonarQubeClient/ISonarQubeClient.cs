namespace TFG.SonarQubeClient
{
	public interface ISonarQubeClient
	{
		IProjectsClient Projects { get; }
		IUsersManagementClient Users { get; }
		IDopTranslationsClient DopTranslations { get; }
		IPermissionsClient Permissions { get; }
	}
}
