using TFG.SonarQubeClient;
using TFG.SonarQubeClient.Models;
using TFG.SonarQubeClient.Models.Metrics;

namespace TFG.Tests.Fakes;

public class FakeSonarQubeClient : ISonarQubeClient
{
    public IProjectsClient Projects { get; } = new FakeProjectsClient();
    public IUsersManagementClient Users { get; } = new FakeUsersManagementClient();
    public IDopTranslationsClient DopTranslations { get; } = new FakeDopTranslationsClient();
    public IPermissionsClient Permissions { get; } = new FakePermissionsClient();

    public bool BoundProjectCalled => ((FakeDopTranslationsClient)DopTranslations).BoundProjectCalled;

    private class FakeProjectsClient : IProjectsClient
    {
        public bool CreateProjectCalled { get; private set; } = false;

        public Task<ProjectCreated> CreateAsync(ProjectCreation projectCreation)
        {
            CreateProjectCalled = true;
            return Task.FromResult(new ProjectCreated());
        }

        public Task DeleteAsync(string projectKey)
        {
            return Task.CompletedTask;
        }

        public Task<SonarMetricsResponse> GetMetrics(SonarMetricsRequest request)
        {
            return Task.FromResult(new SonarMetricsResponse()
            {
                Component = new()
                {
                    Key = request.ProjectKey,
                    Measures = [
                        new()
                        {
                            Metric = SonarMetricKeys.CodeSmells, Value = "100",
                        },
                        new()
                        {
                            Metric = SonarMetricKeys.Bugs, Value = "50",
                        },
                        new()
                        {
                            Metric = SonarMetricKeys.Vulnerabilities, Value = "25",
                        },
                        new()
                        {
                            Metric = SonarMetricKeys.Coverage, Value = "80",
                        },
                        new()
                        {
                            Metric = SonarMetricKeys.Ncloc, Value = "1000",
                        },
                    ],
                    
				}
            });
        }

        public Task<SonarComponentsTree> GetComponentsTree(GetComponentsRequest request)
        {
            return Task.FromResult(new SonarComponentsTree());
        }

        public Task UpdateKeyAsync(UpdateProjectKey updateProjectKey)
        {
            return Task.CompletedTask;
        }
    }

    private class FakeUsersManagementClient : IUsersManagementClient
    {
        public Task<User> CreateAsync(UserCreation userCreation)
        {
            return Task.FromResult(new User());
        }

        public Task DeleteAsync(string userId, bool deleteData)
        {
            return Task.CompletedTask;
        }
    }

    private class FakeDopTranslationsClient : IDopTranslationsClient
    {
        public bool BoundProjectCalled { get; private set; } = false;
		public Task<DopSettingsResponse> GetDopSettingsAsync()
        {
            return Task.FromResult(new DopSettingsResponse() { DopSettings = new() { new() { Id = "123", AppId = "123", Key = "123", Type = "gitlab", Url = "http://gitlab.com"} } });
        }

        public Task<BoundedProject> BoundProjectAsync(ProjectBinding projectBinding)
        {
            BoundProjectCalled = true;
			return Task.FromResult(new BoundedProject());
        }
    }

    private class FakePermissionsClient : IPermissionsClient
    {
        public Task AddUserAsync(UserPermission userPermission)
        {
            return Task.CompletedTask;
        }

        public Task DeleteUserAsync(UserPermission userPermission)
        {
            return Task.CompletedTask;
        }
    }
}
