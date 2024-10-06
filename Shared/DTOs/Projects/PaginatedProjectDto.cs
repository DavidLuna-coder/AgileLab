namespace Shared.DTOs.Projects
{
	public record PaginatedProjectDto
	{
		public string Name { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
