using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TFG.OpenProjectClient.Models.Users
{
	public class UserCreated : HalResource<CreatedUserLink>
	{
		[JsonPropertyName("id")]
		public int Id { get; set; }
		/// <summary>
		/// URL to user's avatar.
		/// </summary>
		[JsonPropertyName("avatar")]
		[Required]
		public string Avatar { get; set; } = string.Empty;

		/// <summary>
		/// Unique login identifier for the user.
		/// Max length: 256 characters.
		/// </summary>
		[JsonPropertyName("login")]
		[MaxLength(256)]
		public string Login { get; set; } = string.Empty;

		/// <summary>
		/// The user's first name.
		/// Max length: 30 characters.
		/// </summary>
		[JsonPropertyName("firstName")]
		[MaxLength(30)]
		public string FirstName { get; set; } = string.Empty;

		/// <summary>
		/// The user's last name.
		/// Max length: 30 characters.
		/// </summary>
		[JsonPropertyName("lastName")]
		[MaxLength(30)]
		public string LastName { get; set; } = string.Empty;

		/// <summary>
		/// The user's email address.
		/// Max length: 60 characters.
		/// </summary>
		[JsonPropertyName("email")]
		[MaxLength(60)]
		[EmailAddress]
		public string Email { get; set; } = string.Empty;

		/// <summary>
		/// Flag indicating whether the user is an admin.
		/// </summary>
		[JsonPropertyName("admin")]
		public bool Admin { get; set; }

		/// <summary>
		/// The current activation status of the user.
		/// </summary>
		[JsonPropertyName("status")]
		public string Status { get; set; } = string.Empty;

		/// <summary>
		/// User's language in ISO 639-1 format.
		/// </summary>
		[JsonPropertyName("language")]
		public string Language { get; set; } = string.Empty;

		/// <summary>
		/// User's identity URL for OmniAuth authentication.
		/// </summary>
		[JsonPropertyName("identityUrl")]
		public string? IdentityUrl { get; set; }

		/// <summary>
		/// Time of creation.
		/// </summary>
		[JsonPropertyName("createdAt")]
		public DateTime CreatedAt { get; set; }

		/// <summary>
		/// Time of the most recent change to the user.
		/// </summary>
		[JsonPropertyName("updatedAt")]
		public DateTime UpdatedAt { get; set; }
	}

	public class CreatedUserLink
	{
		[JsonPropertyName("showUser")]
		public Link ShowUser { get; set; }
	}
}
