namespace TFG.Application.Services.OpenProjectIntegration.Dtos
{
	public class TaskSummaryDto
	{
		public string Name { get; set; }
		public int OpenProjectTaskId { get; set; }
		public string MergeRequestTitle { get; set; }
		public string MergeRequestId { get; set; }
		public string CommitId { get; set; }
		public string SonarQubeAnalisisId { get; set; }
	}
}
