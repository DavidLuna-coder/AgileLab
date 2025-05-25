using Microsoft.Extensions.DependencyInjection;
using NGitLab;
using NuGet.Common;
using Shared.DTOs.Integrations;
using Shared.Utils.DateTimeProvider;
using System;
using TFG.Application.Interfaces;
using TFG.Application.Interfaces.Projects;
using TFG.Application.Services.Auth;
using TFG.Application.Services.OpenProjectIntegration;
using TFG.Application.Services.Projects;
using TFG.Infrastructure.Data;
using TFG.OpenProjectClient;
using TFG.OpenProjectClient.Impl;
using TFG.SonarQubeClient;
using TFG.SonarQubeClient.Impl;

namespace TFG.Application.Services
{
	public static class AppServiceRegistration
	{
		public static IServiceCollection RegisterAppServices(this WebApplicationBuilder builder)
		{
			IServiceCollection services = builder.Services;
			ConfigurationManager configuration = builder.Configuration;
			services.AddHttpContextAccessor();
			services.AddScoped<IAuthService, AuthService>();
			services.AddScoped<IProjectService, ProjectService>();

			services.AddScoped<OpenProjectApi, OpenProjectApi>();


			services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
			services.AddSonarApiClient(serviceProvider =>
				{
					var context = serviceProvider.GetService<ApplicationDbContext>();
					var sonarQubeData = context!.ApiConfigurations.First(t => t.Type == ApiConfigurationType.SonarQube);
					return new SonarHttpClient(sonarQubeData.BaseUrl, sonarQubeData.Token);
				}
			);
			services.AddScoped<IGitLabClient>(serviceProvider =>
			{
				var context = serviceProvider.GetService<ApplicationDbContext>();
				var gitlabdata = context!.ApiConfigurations.First(t => t.Type == ApiConfigurationType.Gitlab);
				var client = new GitLabClient(gitlabdata.BaseUrl, gitlabdata.Token);

				return client;
			});

			services.AddOpenProjectApiClient(services =>
			{
				var context = services.GetService<ApplicationDbContext>();
				var openProjectData = context!.ApiConfigurations.First(t => t.Type == ApiConfigurationType.OpenProject);
				return new OpenProjectHttpClient(openProjectData.BaseUrl, openProjectData.Token);
			});
			return services;
		}
	}
}
