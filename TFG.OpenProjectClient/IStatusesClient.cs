using TFG.OpenProjectClient.Models;
using TFG.OpenProjectClient.Models.Statuses;

namespace TFG.OpenProjectClient
{
	public interface IStatusesClient
	{
		Task<OpenProjectCollection<Status>> GetStatusesAsync();
	}
}
