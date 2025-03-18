using System.Text.Json.Serialization;

namespace TFG.SonarQubeClient.Models
{
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum ProjectVisibility
	{
		[JsonPropertyName("private")]
		Private,

		[JsonPropertyName("public")]
		Public
	}
}
