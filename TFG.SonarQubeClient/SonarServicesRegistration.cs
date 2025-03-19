using Microsoft.Extensions.DependencyInjection;
using TFG.SonarQubeClient.Impl;

namespace TFG.SonarQubeClient
{
	public static class SonarServicesRegistration
    {
		public static void AddSonarApiClient(this IServiceCollection services, string url, string token)
		{
			services.AddSingleton<ISonarHttpClient, SonarHttpClient>(s => new SonarHttpClient(url, token));
			services.AddSingleton<IDopTranslationsClient, DopTranslationsClient>();
			services.AddSingleton<IUsersManagementClient, UsersManagementClient>();
			services.AddSingleton<IPermissionsClient, PermissionsClient>();
			services.AddSingleton<IProjectsClient, ProjectsClient>();
			services.AddSingleton<ISonarQubeClient, SonarClient>();
		}
	}
}
