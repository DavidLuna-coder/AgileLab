using Microsoft.Extensions.DependencyInjection;
using TFG.SonarQubeClient.Impl;

namespace TFG.SonarQubeClient
{
	public static class SonarServicesRegistration
    {
		public static void AddSonarApiClient(this IServiceCollection services, string url, string token)
		{
			services.AddScoped<ISonarHttpClient, SonarHttpClient>(s => new SonarHttpClient(url, token));
			services.AddScoped<IDopTranslationsClient, DopTranslationsClient>();
			services.AddScoped<IUsersManagementClient, UsersManagementClient>();
			services.AddScoped<IPermissionsClient, PermissionsClient>();
			services.AddScoped<IProjectsClient, ProjectsClient>();
			services.AddScoped<ISonarQubeClient, SonarClient>();
		}

		public static void AddSonarApiClient(this IServiceCollection services, Func<IServiceProvider, SonarHttpClient> httpFactory)
		{
			services.AddScoped<ISonarHttpClient>(httpFactory);
			services.AddScoped<IDopTranslationsClient, DopTranslationsClient>();
			services.AddScoped<IUsersManagementClient, UsersManagementClient>();
			services.AddScoped<IPermissionsClient, PermissionsClient>();
			services.AddScoped<IProjectsClient, ProjectsClient>();
			services.AddScoped<ISonarQubeClient, SonarClient>();
		}
	}
}
