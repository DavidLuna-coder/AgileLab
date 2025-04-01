namespace TFG.OpenProjectClient.Models.WorkPackages
{
	public class GetWorkPackagesQuery
	{
		public int ProjectId { get; set; }
		public int? Offset { get; set; }
		public int? PageSize { get; set; }
		public string? Filters { get; set; }
		public string? SortBy { get; set; }
		public string? GroupBy { get; set; }
		public bool? ShowSums { get; set; }
		public string? Select { get; set; }
	}
}
