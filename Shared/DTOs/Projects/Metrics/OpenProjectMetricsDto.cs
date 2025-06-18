namespace Shared.DTOs.Projects.Metrics;

public class OpenProjectMetricsDto
{
	public int TotalTasks { get; set; }
	public int OpenTasks { get; set; }
	public int ClosedTasks { get; set; }
	public int AssignedTasks { get; set; }
	public int CreatedTasks { get; set; }
}
