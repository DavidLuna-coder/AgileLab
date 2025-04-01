using TFG.OpenProjectClient.Models.Users;

namespace TFG.OpenProjectClient
{
	public interface IUsersClient
	{
		Task<UserCreated> CreateAsync(UserCreation userCreation);
		Task DeleteAsync(int userId);
	}
}
