namespace TFG.SonarQubeClient.Models
{
	public class GetComponentsRequest
    {
        public required string ComponentKey { get; set; }
        public string[] Qualifiers { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; } = 1;

		public string BuildRequestUri()
		{
			var qualifiers = string.Join(",", Qualifiers);
			return $"components/tree?component={ComponentKey}&qualifiers={qualifiers}&ps{PageSize}&p={Page}";
		}
	}
}
