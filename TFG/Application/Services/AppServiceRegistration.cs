using TFG.Application.Interfaces;
using TFG.Application.Interfaces.GitlabApiIntegration;
using TFG.Application.Services.Auth;
using TFG.Application.Services.GitlabIntegration;

namespace TFG.Application.Services
{
    public static class AppServiceRegistration
    {
        public static IServiceCollection RegisterAppServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<GitLabApi, GitLabApi>();
            services.AddScoped<IGitlabApiIntegration, GitlabApiIntegration>();
            return services;
        }
    }
}
