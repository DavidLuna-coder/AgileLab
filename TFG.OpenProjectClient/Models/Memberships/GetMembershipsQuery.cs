using System.Web;
using TFG.OpenProjectClient.Models.BasicObjects;

namespace TFG.OpenProjectClient.Models.Memberships
{
	public class GetMembershipsQuery
	{
		public OpenProjectFilters[]? Filters { get; set; }
		public override string ToString()
		{
			var queryParams = HttpUtility.ParseQueryString(string.Empty);

			if (Filters?.Length > 0)
			{
				string filtersJson = "[" + string.Join(",", Filters.Select(f => f.ToString())) + "]";
				queryParams["filters"] = filtersJson;
			}

			return queryParams.ToString() ?? string.Empty;
		}
	}
}
