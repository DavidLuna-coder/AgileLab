namespace Shared.DTOs.Projects
{
	public class UpdateProjectDto
	{
		public string Name { get; set; }
		public string Description { get; set; } = string.Empty;
		public List<string> UsersIds { get; set; } = [];
	}
}
