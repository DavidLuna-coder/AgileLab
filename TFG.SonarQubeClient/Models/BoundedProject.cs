using System.Text.Json.Serialization;

namespace TFG.SonarQubeClient.Models
{
	public class BoundedProject
	{
		[JsonPropertyName("projectId")]
		public string ProjectId { get; set; }

		[JsonPropertyName("bindingId")]
		public string BindingId { get; set; }
	}

}
