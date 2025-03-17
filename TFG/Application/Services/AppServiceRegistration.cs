using NGitLab;
using Shared.Utils.DateTimeProvider;
using TFG.Application.Interfaces;
using TFG.Application.Interfaces.OpenProjectApiIntegration;
using TFG.Application.Interfaces.Projects;
using TFG.Application.Interfaces.SonarQubeIntegration;
using TFG.Application.Services.Auth;
using TFG.Application.Services.OpenProjectIntegration;
using TFG.Application.Services.Projects;
using TFG.Application.Services.SonarQubeIntegration;

namespace TFG.Application.Services
{
    public static class AppServiceRegistration
    {
        public static IServiceCollection RegisterAppServices(this IServiceCollection services)
        {
			services.AddHttpContextAccessor();
			services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IProjectService, ProjectService>();
                       
            services.AddScoped<OpenProjectApi, OpenProjectApi>();
            services.AddScoped<IOpenProjectApiIntegration, OpenProjectApiIntegration>();

            services.AddScoped<SonarQubeApi, SonarQubeApi>();
            services.AddScoped<ISonarQubeApiIntegration, SonarQubeApiIntegration>();

            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

            services.AddScoped<IGitLabClient>(serviceProvider =>
            {
                var configuration = serviceProvider.GetService<IConfiguration>();
                string url = configuration!.GetValue<string>("Gitlab:GitLabBaseAddress") ?? throw new ArgumentNullException();
                string token = configuration.GetValue<string>("Gitlab:GitLabApiKey") ?? throw new ArgumentNullException();
                var client = new GitLabClient(url, token);

                return client;
            });
            return services;
        }
    }
}
