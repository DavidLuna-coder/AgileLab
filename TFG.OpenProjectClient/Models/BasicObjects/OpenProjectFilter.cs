using System.Text.Json;
using System.Text.Json.Serialization;

namespace TFG.OpenProjectClient.Models.BasicObjects
{
	public class OpenProjectFilters()
	{
		[JsonPropertyName("operator")]
		public string Operator { get; set; } = string.Empty;

		[JsonPropertyName("values")]
		public string[] Values { get; set; } = [];

		public string Name { get; set; } = string.Empty;

		public override string ToString()
		{
			var formattedObject = new Dictionary<string, object>
			{
				[Name] = new
				{
					@operator = Operator,  // Usa '@' para evitar conflicto con la palabra clave "operator"
					values = Values.Length > 0 ? Values : null
				}
			};

			return JsonSerializer.Serialize(formattedObject, new JsonSerializerOptions { WriteIndented = false });
		}
	}
}
