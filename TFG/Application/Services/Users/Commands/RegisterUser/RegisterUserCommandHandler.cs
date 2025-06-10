using MediatR;
using Microsoft.AspNetCore.Identity;
using NGitLab;
using NGitLab.Models;
using TFG.OpenProjectClient;
using TFG.OpenProjectClient.Models.Users;
using TFG.SonarQubeClient;
using User = TFG.Domain.Entities.User;

namespace TFG.Application.Services.Users.Commands.RegisterUser
{
	public class RegisterUserCommandHandler(IGitLabClient gitLabClient, IOpenProjectClient openProjectClient, ISonarQubeClient sonarQubeClient, UserManager<User> userManager) : IRequestHandler<RegisterUserCommand>
	{
		public async Task Handle(RegisterUserCommand request, CancellationToken cancellationToken)
		{
			User user = new()
			{
				Email = request.Email,
				EmailConfirmed = true,
				UserName = request.UserName,
				IsAdmin = false,
				FirstName = request.FirstName,
				LastName = request.LastName,
				OpenProjectId = string.Empty,
				GitlabId = string.Empty,
				SonarQubeId = string.Empty,
			};
			try
			{
				user.GitlabId = (await RegisterInGitlab(request)).ToString();
				user.OpenProjectId = (await RegisterOpenProject(request)).Id.ToString();
				user.SonarQubeId = await RegisterSonarQube(request);

				var result = await userManager.CreateAsync(user, request.Password);

				if(!result.Succeeded)
				{
					throw new Exception();
				}
			}
			catch (Exception ex)
			{
				await TryDeleteUserFromServices(user);
				//Borrar en caso de error el registro en las diferentes plataformas
				throw new Exception("An error occurred while registering the user.", ex);
			}
			// Implement the logic to register a user here
			throw new NotImplementedException("User registration logic is not implemented yet.");
		}

		private async Task TryDeleteUserFromServices(User user)
		{
			if (!string.IsNullOrEmpty(user.GitlabId))
			{
				try
				{
					gitLabClient.Users.Delete(long.Parse(user.GitlabId));
				}
				catch { }
			}
			if (!string.IsNullOrEmpty(user.OpenProjectId))
			{
				await SafeDelete(() => openProjectClient.Users.DeleteAsync(int.Parse(user.OpenProjectId)));
			}
			if (!string.IsNullOrEmpty(user.SonarQubeId))
			{
				await SafeDelete(() => sonarQubeClient.Users.DeleteAsync(user.SonarQubeId));
			}
		}

		private async Task<long> RegisterInGitlab(RegisterUserCommand request)
		{
			UserUpsert user = new()
			{
				Email = request.Email,
				Password = request.Password,
				Name = $"{request.FirstName} {request.LastName}",
				Username = request.UserName,
			};

			NGitLab.Models.User gitlabUser = await gitLabClient.Users.CreateAsync(user);
			return gitlabUser.Id;
		}

		private async Task<UserCreated> RegisterOpenProject(RegisterUserCommand model)
		{
			UserCreation userToCreate = new()
			{
				Admin = false,
				Email = model.Email,
				Login = model.UserName,
				FirstName = model.FirstName,
				LastName = model.LastName,
				Language = "es",
				Password = model.Password
			};
			UserCreated userCreated = await openProjectClient.Users.CreateAsync(userToCreate);
			return userCreated;
		}

		private async Task<string> RegisterSonarQube(RegisterUserCommand model)
		{
			SonarQubeClient.Models.UserCreation userCreation = new()
			{
				Login = model.UserName,
				Name = model.FirstName + " " + model.LastName,
				Email = model.Email,
				Password = model.Password,
			};
			var sonarUser = await sonarQubeClient.Users.CreateAsync(userCreation);
			return sonarUser.Id;
		}

		private Task SafeDelete(Func<Task> deleteAction)
		{
			try
			{
				return deleteAction();
			}
			catch
			{
				// Log the error or handle it as needed
				return Task.CompletedTask;
			}
		}
	}
}
