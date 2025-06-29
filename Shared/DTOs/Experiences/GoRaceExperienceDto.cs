namespace Shared.DTOs.Experiences
{
	public class GoRaceExperienceDto
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Token { get; set; }
		public string Description { get; set; }
		public DateTimeOffset CreatedAt { get; set; }
		public string ExperienceType { get; set; } // "Base", "Project", "Platform"
		public Guid? ProjectId { get; set; }
		public List<Guid>? ProjectsIds { get; set; } // For Platform experiences
	}

	public class CreateGoRaceExperienceDto
	{
		public string Name { get; set; }
		public string Token { get; set; }
		public string Description { get; set; }
		public string ExperienceType { get; set; } // "Base", "Project", "Platform"
		public Guid? ProjectId { get; set; }
		public List<Guid>? ProjectsIds { get; set; }
	}

	public class UpdateGoRaceExperienceDto
	{
		public string Name { get; set; }
		public string Token { get; set; }
		public string Description { get; set; }
		public Guid? ProjectId { get; set; }
		public List<Guid>? ProjectsIds { get; set; }
	}
}
