using TFG.OpenProjectClient.Models;
using TFG.OpenProjectClient.Models.WorkPackages;

namespace TFG.OpenProjectClient
{
	public interface IWorkPackagesClient
	{
		Task<OpenProjectCollection<WorkPackage>> GetAsync(GetWorkPackagesQuery query);
	}
}
