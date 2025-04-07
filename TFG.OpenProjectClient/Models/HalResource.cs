using System.Text.Json.Serialization;

namespace TFG.OpenProjectClient.Models
{
	public class HalResource<LinkItem, EmbeddedItem>
	{
		[JsonPropertyName("_type")]
		public string? Type { get; set; }

		[JsonPropertyName("_links")]
		public LinkItem Links { get; set; }

		[JsonPropertyName("_embedded")]
		public EmbeddedItem Embedded { get; set; }
	}
	public class HalResource<LinkItem>
	{
		[JsonPropertyName("_type")]
		public string? Type { get; set; }

		[JsonPropertyName("_links")]
		public LinkItem Links { get; set; }
	}

	public class OpenProjectCollection<T> : HalResource<OpenProjectSelfLink, EmbeddedItems<T>>
	{
		// Meta información de la colección
		[JsonPropertyName("total")]
		public int Total { get; set; }

		[JsonPropertyName("count")]
		public int Count { get; set; }

		[JsonPropertyName("pageSize")]
		public int? PageSize { get; set; }

		[JsonPropertyName("offset")]
		public int? Offset { get; set; }

		[JsonPropertyName("groups")]
		public object? Groups { get; set; }

		[JsonPropertyName("totalSums")]
		public object? TotalSums { get; set; }
	}
	public class OpenProjectSelfLink
	{
		[JsonPropertyName("self")]
		public Link Self { get; set; }
	}
	public class EmbeddedItems<T>
	{
		[JsonPropertyName("elements")]
		public T[] Elements { get; set; }
	}

	public class Link
	{
		[JsonPropertyName("href")]
		public required string? Href { get; set; }
		[JsonPropertyName("title")]
		public string Title { get; set; }
		[JsonPropertyName("templated")]
		public bool Templated { get; set; } = false;
		[JsonPropertyName("method")]
		public string Method { get; set; } = "get";
		[JsonPropertyName("payload")]
		public string Payload { get; set; } = "unspecified";
		[JsonPropertyName("identifier")]
		public string Identifier { get; set; } = "unspecified";
	}
}
