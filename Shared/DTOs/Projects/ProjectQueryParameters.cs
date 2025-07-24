namespace Shared.DTOs.Projects
{
	public class ProjectQueryParameters
	{
		public string? Name { get; set; }
		public List<Guid>? ExperiencesIds { get; set; }
		public List<Guid>? ProjectIds { get; set; }
        public bool? IsArchived { get; set; }
	}
}
