using System.Text.Json.Serialization;

namespace TFG.OpenProjectClient.Models.Statuses
{
	public class StatusLinkedProperties
	{
		[JsonPropertyName("self")]
		public Link Self { get; set; }
	}
}
