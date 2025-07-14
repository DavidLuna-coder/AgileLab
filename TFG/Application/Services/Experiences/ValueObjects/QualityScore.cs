namespace TFG.Application.Services.Experiences.ValueObjects
{
	public record QualityScore(ProjectQualityValues Values, int MaxQualityScore)
	{
		public int CalculateScore()
		{
			double issueRatio = Values.IssueRatio;
			if (issueRatio >= 1) return 0;

			return (int)(MaxQualityScore * (1 - issueRatio));
		}
	}
}
