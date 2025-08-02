using Moq;
using TFG.Application.Services.Projects.Commands.UpdateProject;
using TFG.Infrastructure.Data;
using TFG.OpenProjectClient;
using TFG.SonarQubeClient;
using NGitLab;
using NGitLab.Models;
using Microsoft.Extensions.Logging;
using TFG.Application.Security;
using Microsoft.EntityFrameworkCore;
using User = TFG.Domain.Entities.User;
using Project = TFG.Domain.Entities.Project;

public class UpdateProjectCommandHandlerTests
{
	private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;
	private readonly Mock<IOpenProjectClient> _openProjectClientMock;
	private readonly Mock<IGitLabClient> _gitLabClientMock;
	private readonly Mock<ISonarQubeClient> _sonarQubeClientMock;
	private readonly Mock<IUserInfoAccessor> _userInfoAccessorMock;
	private readonly Mock<ILogger<UpdateProjectCommandHandler>> _loggerMock;

	public UpdateProjectCommandHandlerTests()
	{
		_dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
			.UseInMemoryDatabase(databaseName: "TestDatabase")
			.Options;

		_openProjectClientMock = new Mock<IOpenProjectClient>();
		_gitLabClientMock = new Mock<IGitLabClient>();
		_sonarQubeClientMock = new Mock<ISonarQubeClient>();
		_userInfoAccessorMock = new Mock<IUserInfoAccessor>();
		_loggerMock = new Mock<ILogger<UpdateProjectCommandHandler>>();
	}

	[Fact]
	public async Task Handle_ShouldUpdateProjectSuccessfully()
	{
		// Arrange
		using var dbContext = new ApplicationDbContext(_dbContextOptions);
		var user1 = new User { Id = "user1", UserName = "User 1", FirstName = "First", LastName = "Last", GitlabId = "1", OpenProjectId = "1", SonarQubeId = "1" };
		var user2 = new User { Id = "user2", UserName = "User 2", Email = "test2@mail.com", FirstName = "First", LastName = "Last", GitlabId = "2", OpenProjectId = "2", SonarQubeId = "2" };

		dbContext.Users.Add(user1);
		dbContext.Users.Add(user2);
		dbContext.SaveChanges();

		var project = new Project
		{
			Id = Guid.NewGuid(),
			Name = "Old Project",
			Description = "Old Description",
			GitlabId = "1",
			SonarQubeProjectKey = "key",
			Users = new List<User> { user1 }
		};

		dbContext.Projects.Add(project);
		dbContext.SaveChanges();

		_gitLabClientMock.Setup(client => client.Members.RemoveMemberFromProjectAsync(It.IsAny<ProjectId>(), It.IsAny<long>(), It.IsAny<CancellationToken>()))
			.Returns(Task.CompletedTask);

		_gitLabClientMock.Setup(client => client.Members.AddMemberToProjectAsync(It.IsAny<ProjectId>(), It.IsAny<ProjectMemberCreate>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(new Membership());

		var handler = new UpdateProjectCommandHandler(
			dbContext,
			_openProjectClientMock.Object,
			_gitLabClientMock.Object,
			_sonarQubeClientMock.Object,
			_userInfoAccessorMock.Object,
			_loggerMock.Object
		);

		var command = new UpdateProjectCommand
		{
			ProjectId = project.Id,
			Name = "Updated Project",
			Description = "Updated Description",
			UsersIds = new List<string> { "user1", "user2" }
		};

		// Act
		var result = await handler.Handle(command, CancellationToken.None);

		// Assert
		Assert.NotNull(result);
		Assert.Equal("Updated Project", result.Name);
		Assert.Equal(2, dbContext.Projects.Include(p => p.Users).First().Users.Count);
	}
}