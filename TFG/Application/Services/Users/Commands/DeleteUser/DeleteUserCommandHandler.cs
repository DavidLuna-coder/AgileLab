using MediatR;
using NGitLab;
using TFG.Api.Exeptions;
using TFG.Infrastructure.Data;
using TFG.OpenProjectClient;
using TFG.SonarQubeClient;

namespace TFG.Application.Services.Users.Commands.DeleteUser
{
	public class DeleteUserCommandHandler(ApplicationDbContext context, IGitLabClient gitLabClient, ISonarQubeClient sonarQubeClient, IOpenProjectClient openProjectClient, ILogger<DeleteUserCommandHandler> logger) : IRequestHandler<DeleteUserCommand>
	{
		public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
		{
			var user = await context.Users.FindAsync(request.UserId)
				?? throw new NotFoundException("El usuario no existe");


			List<Task> deletionTasks = [TryDeleteGitLabUser(user.GitlabId), TryDeleteSonarQubeUser(user.SonarQubeId), TryDeleteOpenProjectUser(user.OpenProjectId)];

			await Task.WhenAll(deletionTasks);
			context.Users.Remove(user);

			await context.SaveChangesAsync(cancellationToken);
		}

		private Task<bool> TryDeleteGitLabUser(string gitlabId)
		{
			if (string.IsNullOrEmpty(gitlabId)) return Task.FromResult(false);
			try
			{
				gitLabClient.Users.Delete(long.Parse(gitlabId));
				return Task.FromResult(true);
			}
			catch (Exception ex)
			{
				logger.LogWarning(ex, "Error deleting GitLab user with ID {GitlabId}", gitlabId);
				return Task.FromResult(false);
			}
		}

		private async Task<bool> TryDeleteOpenProjectUser(string openProjectId)
		{
			if (string.IsNullOrEmpty(openProjectId)) return false;
			try
			{
				await openProjectClient.Users.DeleteAsync(int.Parse(openProjectId));
				return true;
			}
			catch (Exception ex)
			{
				logger.LogWarning(ex, "Error deleting OpenProject user with ID {OpenProjectId}", openProjectId);
				return false;
			}
		}

		private async Task<bool> TryDeleteSonarQubeUser(string sonarQubeId)
		{
			if (string.IsNullOrEmpty(sonarQubeId)) return false;
			try
			{
				await sonarQubeClient.Users.DeleteAsync(sonarQubeId);
				return true;
			}
			catch (Exception ex)
			{
				logger.LogWarning(ex, "Error deleting SonarQube user with ID {SonarQubeId}", sonarQubeId);
				return false;
			}
		}
	}
}
