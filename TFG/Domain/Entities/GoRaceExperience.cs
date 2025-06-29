namespace TFG.Domain.Entities;

public class GoRaceExperience
{
	public Guid Id { get; set; }
	public string Name { get; set; }
	public string Token { get; set; }
	public string Description { get; set; }
	public DateTimeOffset CreatedAt { get; set; }
}
public class GoRaceProjectExperience : GoRaceExperience
{
	public Guid ProjectId { get; set; }
	public Project Project { get; set; }
}

public class GoRacePlatformExperience : GoRaceExperience
{
	public ICollection<Project> Projects { get; set; } = [];
}
