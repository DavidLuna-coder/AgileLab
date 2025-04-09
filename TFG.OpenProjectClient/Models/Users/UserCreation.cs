using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TFG.OpenProjectClient.Models.Users
{
	public class UserCreation
	{
		/// <summary>
		/// Unique login identifier for the user.
		/// Max length: 256 characters.
		/// </summary>
		[JsonPropertyName("login")]
		[Required]
		[MaxLength(256)]
		public string Login { get; set; } = string.Empty;

		/// <summary>
		/// The user's password.  
		/// **Conditions:** Only writable on creation, not on update.
		/// </summary>
		[JsonPropertyName("password")]
		[Required]
		public string Password { get; set; } = string.Empty;

		/// <summary>
		/// The user's first name.
		/// Max length: 30 characters.
		/// </summary>
		[JsonPropertyName("firstName")]
		[Required]
		[MaxLength(30)]
		public string FirstName { get; set; } = string.Empty;

		/// <summary>
		/// The user's last name.
		/// Max length: 30 characters.
		/// </summary>
		[JsonPropertyName("lastName")]
		[Required]
		[MaxLength(30)]
		public string LastName { get; set; } = string.Empty;

		/// <summary>
		/// The user's email address.
		/// Max length: 60 characters.
		/// </summary>
		[JsonPropertyName("email")]
		[Required]
		[EmailAddress]
		[MaxLength(60)]
		public string Email { get; set; } = string.Empty;

		/// <summary>
		/// Indicates if the user has admin privileges.
		/// </summary>
		[JsonPropertyName("admin")]
		public bool Admin { get; set; }

		/// <summary>
		/// The current activation status of the user.  
		/// **Conditions:** Only writable on creation, not on update.
		/// </summary>
		[JsonPropertyName("status")]
		public string Status { get; set; } = "active";

		/// <summary>
		/// The preferred language of the user.
		/// </summary>
		[JsonPropertyName("language")]
		public string Language { get; set; } = string.Empty;
	}
}
