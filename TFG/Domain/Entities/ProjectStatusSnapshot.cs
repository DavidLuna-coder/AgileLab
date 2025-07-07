namespace TFG.Domain.Entities;

public class ProjectStatusSnapshot
{
	public Guid Id { get; set; }
	public Guid ProjectId { get; set; }
	public Project Project { get; set; }
	public DateTime SnapshotDate { get; set; }
	public int Bugs { get; set; }
	public int Vulnerabilities { get; set; }
	public int CodeSmells { get; set; }
	public int Loc { get; set; }
	public int ClosedTasks { get; set; }
	public int OpenTasks { get; set; }
	public int OverdueTasks { get; set; }
	public int MergeRequestsCreated { get; set; }
	public int MergeRequestsMerged { get; set; }
}

public class UserProjectStatusSnapshot
{
	public Guid Id { get; set; }
	public Guid ProjectId { get; set; }
	public Project Project { get; set; }
	public string UserId { get; set; }
	public User User { get; set; }
	public DateTime SnapshotDate { get; set; }
	public int ClosedTasks { get; set; }
	public int OpenTasks { get; set; }
	public int OverdueTasks { get; set; }
	public int MergeRequestsCreated { get; set; }
	public int MergeRequestsMerged { get; set; }
}
