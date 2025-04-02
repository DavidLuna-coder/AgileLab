using System.Web;
using TFG.OpenProjectClient.Models.BasicObjects;

namespace TFG.OpenProjectClient.Models.WorkPackages
{
	public class GetWorkPackagesQuery
	{
		public int? Offset { get; set; }
		public int? PageSize { get; set; }
		public OpenProjectFilters[]  Filters { get; set; }
		public string? SortBy { get; set; }
		public string? GroupBy { get; set; }
		public bool? ShowSums { get; set; }
		public string? Select { get; set; }

		public override string ToString()
		{
			var queryParams = HttpUtility.ParseQueryString(string.Empty);
			if (Offset.HasValue) queryParams["offset"] = Offset.Value.ToString();
			if (PageSize.HasValue) queryParams["pageSize"] = PageSize.Value.ToString();
			if (!string.IsNullOrEmpty(SortBy)) queryParams["sortBy"] = SortBy;
			if (!string.IsNullOrEmpty(GroupBy)) queryParams["groupBy"] = GroupBy;
			if (ShowSums.HasValue) queryParams["showSums"] = ShowSums.Value.ToString().ToLower();
			if (!string.IsNullOrEmpty(Select)) queryParams["select"] = Select;

			if (Filters?.Length > 0)
			{
				string filtersJson = "[" + string.Join(",", Filters.Select(f => f.ToString())) + "]";
				queryParams["filters"] = filtersJson;
			}

			return queryParams.ToString() ?? string.Empty;
		}
	}
}
