using TFG.Application.Interfaces;
using TFG.Application.Interfaces.GitlabApiIntegration;
using TFG.Application.Interfaces.OpenProjectApiIntegration;
using TFG.Application.Services.Auth;
using TFG.Application.Services.GitlabIntegration;
using TFG.Application.Services.OpenProjectIntegration;

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
            return services;
        }
    }
}
