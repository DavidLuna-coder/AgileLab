namespace TFG.Application.Services.Experiences.ValueObjects
{
	public record ImprovementScore(ProjectQualityValues LatestValues, ProjectQualityValues PreviousValues, int ImprovementFactor)
	{
		public int CalculateScore()
		{
			if (LatestValues.TotalIssues >= PreviousValues.TotalIssues) return 0;
			int totalImprovement = PreviousValues.TotalIssues - LatestValues.TotalIssues;
			return totalImprovement * ImprovementFactor;
		}
	}
}
