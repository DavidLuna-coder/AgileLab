using TFG.OpenProjectClient;
using TFG.OpenProjectClient.Models.Projects;
using TFG.OpenProjectClient.Models.Users;
using TFG.OpenProjectClient.Models.Memberships;
using TFG.OpenProjectClient.Models;

namespace TFG.Tests.Fakes;

public class FakeOpenProjectClient : IOpenProjectClient
{
	public FakeOpenProjectClient()
	{
		Users = new FakeUsersClient();
		Projects = new FakeProjectsClient();
		Memberships = new FakeMembershipsClient();
	}

	public IUsersClient Users { get; }
	public IProjectsClient Projects { get; }
	public IMembershipsClient Memberships { get; }
    public IStatusesClient Statuses => throw new NotImplementedException();
    public IWorkPackagesClient WorkPackages => throw new NotImplementedException();

	public bool CreateProjectCalled => ((FakeProjectsClient)Projects).CreateProjectCalled;

	private class FakeProjectsClient : IProjectsClient
	{
		public bool CreateProjectCalled { get; private set; } = false;

		public Task<ProjectCreated> CreateAsync(ProjectCreation projectCreation)
		{
			CreateProjectCalled = true;
			return Task.FromResult(new ProjectCreated
			{
				Id = 1,
				Name = projectCreation.Name,
				Description = projectCreation.Description,
				Identifier = projectCreation.Identifier
			});
		}

		public Task DeleteAsync(int projectId)
		{
			return Task.CompletedTask;
		}

		public Task UpdateAsync(ProjectUpdate projectUpdate)
		{
			return Task.CompletedTask;
		}
	}

	private class FakeUsersClient : IUsersClient
	{
		public Task<UserCreated> CreateAsync(UserCreation userCreation)
		{
			return Task.FromResult(new UserCreated
			{
				Id = 1,
				Login = userCreation.Login,
				FirstName = userCreation.FirstName,
				LastName = userCreation.LastName,
				Email = userCreation.Email
			});
		}

		public Task DeleteAsync(int userId)
		{
			return Task.CompletedTask;
		}
	}

	private class FakeMembershipsClient : IMembershipsClient
	{
		public Task<Membership> CreateAsync(MembershipCreation membershipCreation)
		{
			return Task.FromResult(new Membership
			{
				Id = 1,
			});
		}

		public Task DeleteAsync(int membershipId)
		{
			return Task.CompletedTask;
		}

		public Task<OpenProjectCollection<Membership>> GetAsync(GetMembershipsQuery? query = null)
		{
			return Task.FromResult(new OpenProjectCollection<Membership>());
		}
	}

}