using System.Text.Json.Serialization;

namespace TFG.SonarQubeClient.Models
{
	public class SonarPaging
    {
        [JsonPropertyName("pageIndex")]
        public int PageIndex { get; set; }
        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; }
        [JsonPropertyName("total")]
        public int Total { get; set; }
    }
}
