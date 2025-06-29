using Blazored.LocalStorage;
using Front;
using Front.ApiClient.Implementations;
using Front.ApiClient.Interfaces;
using Front.Services.StateService;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Shared.Utils.DateTimeProvider;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddMudServices();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<IDateTimeProvider, DateTimeProvider>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IApiHttpClient, ApiHttpClient>();
builder.Services.AddScoped<IProjectsApi, ProjectsApi>();
builder.Services.AddScoped<IUsersApi, UsersApi>();
builder.Services.AddScoped<IApiIntegrationsApi, ApiIntegrationsApi>();
builder.Services.AddScoped<IRolesApi, RolesApi>();
builder.Services.AddScoped<IGoRaceExperiencesApi, Front.ApiClient.Implementations.GoRaceExperiences.GoRaceExperiencesApi>();
builder.Services.AddScoped<ProjectStateService>();

await builder.Build().RunAsync();
