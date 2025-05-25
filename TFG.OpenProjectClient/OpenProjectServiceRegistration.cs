using Microsoft.Extensions.DependencyInjection;
using TFG.OpenProjectClient.Impl;

namespace TFG.OpenProjectClient
{
	public static class OpenProjectServiceRegistration
	{
		public static void AddOpenProjectApiClient(this IServiceCollection services, string url, string token)
		{
			if (url is null || token is null) throw new ArgumentNullException("OpenProject URL or token cannot be null");

			services.AddScoped<IOpenProjectHttpClient, OpenProjectHttpClient>(s => new OpenProjectHttpClient(url, token));
			services.AddScoped<IProjectsClient, ProjectClient>();
			services.AddScoped<IUsersClient, UserClient>();
			services.AddScoped<IWorkPackagesClient, WorkPackagesClient>();
			services.AddScoped<IMembershipsClient, MembershipClient>();
			services.AddScoped<IStatusesClient, StatusesClient>();
			services.AddScoped<IOpenProjectClient, OpenProjectClientImpl>();
		}

		public static void AddOpenProjectApiClient(this IServiceCollection services, Func<IServiceProvider, OpenProjectHttpClient> factory)
		{
			services.AddScoped<IOpenProjectHttpClient>(factory);
			services.AddScoped<IProjectsClient, ProjectClient>();
			services.AddScoped<IUsersClient, UserClient>();
			services.AddScoped<IWorkPackagesClient, WorkPackagesClient>();
			services.AddScoped<IMembershipsClient, MembershipClient>();
			services.AddScoped<IStatusesClient, StatusesClient>();
			services.AddScoped<IOpenProjectClient, OpenProjectClientImpl>();
		}
	}
}
