namespace Shared.DTOs.Users
{
	public class GetUsersQueryParameters
	{
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public string? Email { get; set; }
		public List<string>? Ids { get; set; }

	}
}
