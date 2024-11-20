namespace Shared.DTOs.Projects
{
	public record ProjectDto
	{
		public required Guid Id { get; set; }
		public required string Name { get; set; } = string.Empty;
		public required string Description { get; set; } = string.Empty;
		public required List<string> UsersIds { get; set; } = [];
		public required DateTime CreatedAt { get; set; }
	}
}
