using Shared.Utils.DateTimeProvider;
using TFG.Application.Interfaces;
using TFG.Application.Interfaces.GitlabApiIntegration;
using TFG.Application.Interfaces.OpenProjectApiIntegration;
using TFG.Application.Interfaces.SonarQubeIntegration;
using TFG.Application.Services.Auth;
using TFG.Application.Services.GitlabIntegration;
using TFG.Application.Services.OpenProjectIntegration;
using TFG.Application.Services.SonarQubeIntegration;

namespace TFG.Application.Services
{
    public static class AppServiceRegistration
    {
        public static IServiceCollection RegisterAppServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            
            services.AddScoped<GitLabApi, GitLabApi>();
            services.AddScoped<IGitlabApiIntegration, GitlabApiIntegration>();
            
            services.AddScoped<OpenProjectApi, OpenProjectApi>();
            services.AddScoped<IOpenProjectApiIntegration, OpenProjectApiIntegration>();

            services.AddScoped<SonarQubeApi, SonarQubeApi>();
            services.AddScoped<ISonarQubeApiIntegration, SonarQubeApiIntegration>();

            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            return services;
        }
    }
}
