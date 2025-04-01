using System.Text.Json.Serialization;

namespace TFG.OpenProjectClient.Models.BasicObjects
{
	public class FormattableText
	{
		[JsonPropertyName("format")]
		public string Format { get; set; } = "plain";
		[JsonPropertyName("raw")]
		public string Raw { get; set; }
		[JsonPropertyName("html")]
		public string Html { get; set; }
	}
}
