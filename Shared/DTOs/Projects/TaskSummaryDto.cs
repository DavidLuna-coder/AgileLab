namespace Shared.DTOs.Projects
{
	public class TaskSummaryDto
	{
		public string Name { get; set; }
		public int OpenProjectTaskId { get; set; }
		public List<TaskSummaryMergeRequestInfo> MergeRequests { get; set; } = new();
		public string AsignedTo { get; set; }
		public string Author { get; set; }
		public bool IsOverdue { get; set; } // Nueva propiedad para indicar si la tarea está vencida
	}

	public class TaskSummaryMergeRequestInfo
	{
		public string Title { get; set; }
		public long Id { get; set; }
		public List<string> CommitIds { get; set; } = new();
	}

	public class TaskSummaryCommitInfo
	{
		public string Id { get; set; }
		public string Title { get; set; }
		public string Author { get; set; }
		public DateTime CreatedAt { get; set; }
		public string Message { get; set; }
		public string WebUrl { get; set; }
		public string SonarQubeAnalisisId { get; set; }
	}
}
