using Front.ApiClient.Interfaces;
using Shared.Enums;

namespace Front.Services.StateService
{
	public class UserService(IUsersApi usersApi) : IUserService
	{
		public Permissions? Permissions { get; private set; }

		public async Task InitializeAsync()
		{
			if (Permissions is null || Permissions == 0)
				Permissions = await usersApi.GetMyPermissions();
			Console.WriteLine(Permissions);
		}

	}
}
