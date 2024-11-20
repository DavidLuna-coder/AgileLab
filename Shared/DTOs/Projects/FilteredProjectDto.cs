namespace Shared.DTOs.Projects
{
	public record FilteredProjectDto
	{
		public required Guid Id { get; set; }
		public required string Name { get; set; }
		public required DateTime CreatedAt { get; set; }
	}
}
