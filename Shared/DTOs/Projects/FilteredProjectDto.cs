namespace Shared.DTOs.Projects
{
	public record FilteredProjectDto
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
