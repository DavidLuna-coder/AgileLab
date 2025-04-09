namespace TFG.Application.Services.OpenProjectIntegration.Dtos
{
	public class TaskSummaryDto
	{
		public string Name { get; set; }
		public int OpenProjectTaskId { get; set; }
		public string[] MergeRequestsTitles { get; set; }
		public int[] MergeRequestsIds { get; set; }
		public string CommitId { get; set; }
		public string SonarQubeAnalisisId { get; set; }
	}
}
