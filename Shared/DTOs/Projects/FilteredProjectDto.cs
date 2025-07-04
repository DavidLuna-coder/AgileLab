namespace Shared.DTOs.Projects
{
	public record FilteredProjectDto
	{
		public required Guid Id { get; set; }
		public required string Name { get; set; }
		public required DateTime CreatedAt { get; set; }
		public List<UserReferenceDto> Members { get; set; } = new();
	}

	public record UserReferenceDto
	{
		public required string Id { get; set; }
		public required string Email { get; set; }
	}
}
