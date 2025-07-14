namespace TFG.Application.Services.Experiences.ValueObjects
{
	public record ExperienceSendableData(QualityScore QualityScore, ImprovementScore ImprovementScore, OnTimeTaskScore OnTimeTaskScore, int ClosedTasks, int MergedMergeRequest);
}
