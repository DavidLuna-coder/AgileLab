namespace Shared.DTOs.Experiences
{
	public class GoRaceExperienceDto
	{
		public Guid Id { get; set; }
		public string Url { get; set; }
		public string Name { get; set; }
		public string Token { get; set; }
		public string Description { get; set; }
		public DateTimeOffset CreatedAt { get; set; }
		public string ExperienceType { get; set; } // "Base", "Project", "Platform"
		public Guid? ProjectId { get; set; }
		public List<ProjectOwnerDto>? ProjectOwners { get; set; } // For Platform experiences
		public int MaxQualityScore { get; set; }
		public int ImprovementScoreFactor { get; set; }
		public int MaxOnTimeTasksScore { get; set; }
	}

	public class CreateGoRaceExperienceDto
	{
		public string Name { get; set; }
		public string Url { get; set; }
		public string? Token { get; set; }
		public string? Description { get; set; }
		public string ExperienceType { get; set; } // "Base", "Project", "Platform"
		public Guid? ProjectId { get; set; }
		public List<ProjectOwnerDto>? ProjectOwners { get; set; }
		public int MaxQualityScore { get; set; } = 100; // Default value
		public int ImprovementScoreFactor { get; set; } = 1; // Default value
		public int MaxOnTimeTasksScore { get; set; } = 100; // Default value
	}

	public class UpdateGoRaceExperienceDto
	{
		public string Name { get; set; }
		public string Url { get; set; }
		public string? Token { get; set; }
		public string? Description { get; set; }
		public Guid? ProjectId { get; set; }
		public List<ProjectOwnerDto>? ProjectOwners { get; set; }
		public int MaxQualityScore { get; set; } = 100; // Default value
		public int ImprovementScoreFactor { get; set; } = 1; // Default value
		public int MaxOnTimeTasksScore { get; set; } = 100; // Default value
	}

	public class ProjectOwnerDto 
	{
		public Guid ProjectId { get; set; }
		public string Email { get; set; }
	}
}
