using TFG.Application.Services.Projects.Commands.CreateProject;
using TFG.Infrastructure.Data;
using TFG.OpenProjectClient;
using TFG.SonarQubeClient;
using NGitLab;
using Microsoft.Extensions.Logging;
using TFG.Application.Security;
using Microsoft.EntityFrameworkCore;
using User = TFG.Domain.Entities.User;
using TFG.Tests.Fakes;
using TFG.Application.Dtos;
using Microsoft.Extensions.Logging.Abstractions;
using TFG.Api.Exeptions;

public class CreateProjectCommandHandlerTests : IDisposable
{
	private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;
	private IUserInfoAccessor _userInfoAccessor;
	private readonly IGitLabClient _gitLabClient;
	private readonly ISonarQubeClient _sonarQubeClient;
	private readonly IOpenProjectClient _openProjectClient;
	private readonly ILogger<CreateProjectCommandHandler> _logger;

	public CreateProjectCommandHandlerTests()
	{
		_dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
			.UseInMemoryDatabase(databaseName: "TestDatabase")
			.Options;

		_userInfoAccessor = new FakeUserInfoAccessor();
		_gitLabClient = new FakeGitLabClient();
		_sonarQubeClient = new FakeSonarQubeClient();
		_openProjectClient = new FakeOpenProjectClient();
		_logger = NullLogger<CreateProjectCommandHandler>.Instance;

		// Seed data
		using var dbContext = new ApplicationDbContext(_dbContextOptions);
		dbContext.Users.AddRange(
			new User { Id = "user1", UserName = "User 1", Email = "user1@example.com", FirstName = "First", LastName = "Last", GitlabId = "1", OpenProjectId = "1", SonarQubeId = "1" },
			new User { Id = "user2", UserName = "User 2", FirstName = "First", LastName = "Last", GitlabId = "2", OpenProjectId = "2", SonarQubeId = "2" }
		);
		dbContext.SaveChanges();
	}

	public void Dispose()
	{
		using var dbContext = new ApplicationDbContext(_dbContextOptions);
		dbContext.Database.EnsureDeleted();
	}

	[Fact]
	public async Task Handle_ShouldCreateProjectSuccessfully()
	{
		// Arrange
		_userInfoAccessor = new FakeUserInfoAccessor
		{
			UserInfo = new UserInfo { UserId = "user1", Email = "user1@example.com", Username = "User 1", IsAdmin = false }
		};
		using var dbContext = new ApplicationDbContext(_dbContextOptions);

		var handler = new CreateProjectCommandHandler(
			_userInfoAccessor,
			dbContext,
			_gitLabClient,
			_sonarQubeClient,
			_openProjectClient,
			_logger
		);

		var command = new CreateProjectCommand
		{
			Name = "Test Project",
			Description = "Test Description",
			UsersIds = new List<string> { "user1", "user2" }
		};

		// Act
		var result = await handler.Handle(command, CancellationToken.None);

		// Assert
		Assert.NotNull(result);
		Assert.Equal("Test Project", result.Name);
		Assert.Equal(2, dbContext.Projects.Include(p => p.Users).First().Users.Count);
	}

	[Fact]
	public async Task Handle_ShouldCallFakeClientsMethodsSuccessfully()
	{
		// Arrange
		_userInfoAccessor = new FakeUserInfoAccessor
		{
			UserInfo = new UserInfo { UserId = "user1", Email = "user1@example.com", Username = "User 1", IsAdmin = false }
		};
		using var dbContext = new ApplicationDbContext(_dbContextOptions);

		var handler = new CreateProjectCommandHandler(
			_userInfoAccessor,
			dbContext,
			_gitLabClient,
			_sonarQubeClient,
			_openProjectClient,
			_logger
		);

		var command = new CreateProjectCommand
		{
			Name = "Test Project",
			Description = "Test Description",
			UsersIds = new List<string> { "user1", "user2" }
		};

		// Act
		var result = await handler.Handle(command, CancellationToken.None);

		// Assert
		Assert.NotNull(result);
		Assert.Equal("Test Project", result.Name);
		Assert.Equal(2, dbContext.Projects.Include(p => p.Users).First().Users.Count);

		// Verify that the fake clients' methods were called
		var fakeGitLabClient = (FakeGitLabClient)_gitLabClient;
		Assert.True(fakeGitLabClient.CreateProjectCalled);

		var fakeSonarQubeClient = (FakeSonarQubeClient)_sonarQubeClient;
		Assert.True(fakeSonarQubeClient.BoundProjectCalled);

		var fakeOpenProjectClient = (FakeOpenProjectClient)_openProjectClient;
		Assert.True(fakeOpenProjectClient.CreateProjectCalled);
	}

	[Fact]
	public async Task Handle_ShouldRollbackOnGitLabFailure()
	{
		// Arrange
		_userInfoAccessor = new FakeUserInfoAccessor
		{
			UserInfo = new UserInfo { UserId = "user1", Email = "user1@example.com", Username = "User 1", IsAdmin = false }
		};
		using var dbContext = new ApplicationDbContext(_dbContextOptions);

		var fakeGitLabClient = (FakeGitLabClient)_gitLabClient;
		fakeGitLabClient.ShouldFailOnCreate = true;

		var handler = new CreateProjectCommandHandler(
			_userInfoAccessor,
			dbContext,
			_gitLabClient,
			_sonarQubeClient,
			_openProjectClient,
			_logger
		);

		var command = new CreateProjectCommand
		{
			Name = "Test Project",
			Description = "Test Description",
			UsersIds = new List<string> { "user1", "user2" }
		};

		// Act & Assert
		await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));

		// Verify rollback
		Assert.False(dbContext.Projects.Any());
	}

	[Fact]
	public async Task Handle_ShouldThrowExceptionWhenUserDoesNotExist()
	{
		// Arrange
		_userInfoAccessor = new FakeUserInfoAccessor
		{
			UserInfo = new UserInfo { UserId = "user1", Email = "user1@example.com", Username = "User 1", IsAdmin = false }
		};
		using var dbContext = new ApplicationDbContext(_dbContextOptions);

		var handler = new CreateProjectCommandHandler(
			_userInfoAccessor,
			dbContext,
			_gitLabClient,
			_sonarQubeClient,
			_openProjectClient,
			_logger
		);

		var command = new CreateProjectCommand
		{
			Name = "Test Project",
			Description = "Test Description",
			UsersIds = new List<string> { "user1", "user3" } // user3 does not exist
		};

		// Act & Assert
		var exception = await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
		Assert.Equal("One or more users do not exist", exception.Message);
	}
}