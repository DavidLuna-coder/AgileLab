namespace Shared.DTOs.Projects.Metrics;

public class GitlabMetricsDto
{
	public string? UserId { get; set; }
	public int TotalCommits { get; set; }
	public int CreatedMergeRequests { get; set; }
	public int MergedMergeRequests { get; set; }
	public double CommitsPerWeek { get; set; }
	public int TotalIssues { get; set; }
	public int ClosedIssues { get; set; }
}
