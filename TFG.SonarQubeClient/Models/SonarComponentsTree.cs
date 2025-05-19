using System.Text.Json.Serialization;

namespace TFG.SonarQubeClient.Models
{
	public class SonarComponentsTree
    {
        [JsonPropertyName("paging")]
        public SonarPaging Paging { get; set; }
		[JsonPropertyName("components")]
        public List<SonarComponent> Components { get; set; }

	}
}
