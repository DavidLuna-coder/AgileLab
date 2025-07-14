namespace TFG.Application.Services.Experiences.ValueObjects
{
	public record ProjectQualityValues(int Bugs, int Vulnerabilities, int CodeSmells, int Loc)
	{
		public int TotalIssues => Bugs + Vulnerabilities + CodeSmells;
		public double IssueRatio => Loc == 0 ? 0 : (double)TotalIssues / Loc;
	};
}
