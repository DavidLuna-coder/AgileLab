namespace Shared.DTOs.Projects
{
    public record ProjectTasksDto
    {
		public ProjectTaskDto[] Tasks = [];
	}

	public record ProjectTaskDto
	{
		public required string Name { get; set; }
		public int PercentageDone { get; set; } 
	}
}
