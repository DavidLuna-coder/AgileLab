namespace TFG.Domain.Entities;

public class GoRaceExperience
{
	public Guid Id { get; set; }
	public string Name { get; set; }
	public string? Token { get; set; }
	public string? Description { get; set; }
	public DateTimeOffset CreatedAt { get; set; }
	public int MaxQualityScore { get; set; }
	public int ImprovementScoreFactor { get; set; }
	public int MaxOnTimeTasksScore { get; set; }
}
public class GoRaceProjectExperience : GoRaceExperience
{
	public Guid ProjectId { get; set; }
	public Project Project { get; set; }
}

public class GoRacePlatformExperience : GoRaceExperience
{
	public ICollection<GoRacePlatformExperienceProject> Projects { get; set; }
}

public class GoRacePlatformExperienceProject
{
	public Guid Id { get; set; }
	public Guid GoRacePlatformExperienceId { get; set; }
	public GoRacePlatformExperience GoRacePlatformExperience { get; set; }
	public Guid ProjectId { get; set; }
	public Project Project { get; set; }
	public string? OwnerEmail { get; set; }
}
