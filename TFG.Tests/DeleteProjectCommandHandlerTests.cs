using Moq;
using TFG.Application.Services.Projects.Commands.DeleteProject;
using TFG.Infrastructure.Data;
using TFG.OpenProjectClient;
using TFG.SonarQubeClient;
using NGitLab;
using Microsoft.Extensions.Logging;
using TFG.Application.Security;
using Microsoft.EntityFrameworkCore;
using TFG.Domain.Entities;

public class DeleteProjectCommandHandlerTests
{
    private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;
    private readonly Mock<IUserInfoAccessor> _userInfoAccessorMock;
    private readonly Mock<IGitLabClient> _gitLabClientMock;
    private readonly Mock<ISonarQubeClient> _sonarQubeClientMock;
    private readonly Mock<IOpenProjectClient> _openProjectClientMock;
    private readonly Mock<ILogger<DeleteProjectCommandHandler>> _loggerMock;

    public DeleteProjectCommandHandlerTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _userInfoAccessorMock = new Mock<IUserInfoAccessor>();
        _gitLabClientMock = new Mock<IGitLabClient>();
        _sonarQubeClientMock = new Mock<ISonarQubeClient>();
        _openProjectClientMock = new Mock<IOpenProjectClient>();
        _loggerMock = new Mock<ILogger<DeleteProjectCommandHandler>>();
    }

    [Fact]
    public async Task Handle_ShouldDeleteProjectSuccessfully()
    {
        // Arrange
        using var dbContext = new ApplicationDbContext(_dbContextOptions);
        var project = new Project
        {
            Id = Guid.NewGuid(),
            Name = "Test Project",
            Description = "Test Description",
            GitlabId = "1",
            SonarQubeProjectKey = "key"
        };
        dbContext.Projects.Add(project);
        await dbContext.SaveChangesAsync();

        var handler = new DeleteProjectCommandHandler(
            dbContext,
            _userInfoAccessorMock.Object,
            _gitLabClientMock.Object,
            _sonarQubeClientMock.Object,
            _openProjectClientMock.Object,
            _loggerMock.Object
        );

        var command = new DeleteProjectCommand
        {
            ProjectId = project.Id
        };

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        var deletedProject = dbContext.Projects.FirstOrDefault(p => p.Id == command.ProjectId);
        Assert.Null(deletedProject);
    }
}