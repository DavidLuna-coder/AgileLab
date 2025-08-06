namespace Shared.DTOs.Users
{
	public record FilteredUserDto
	{
		public string Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public bool IsAdmin { get; set; }
		public List<FilteredUserRolDto> Roles { get; set; }
	}
	public record FilteredUserRolDto
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
	}
}
