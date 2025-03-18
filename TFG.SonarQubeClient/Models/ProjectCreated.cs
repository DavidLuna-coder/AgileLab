using System.Text.Json.Serialization;

namespace TFG.SonarQubeClient.Models
{
    public class ProjectCreated
    {
		[JsonPropertyName("project")]
		public ProjectCreatedDetails Project { get; set; } = new();
	}
}
