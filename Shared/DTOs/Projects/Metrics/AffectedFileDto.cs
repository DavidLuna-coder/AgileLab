namespace Shared.DTOs.Projects.Metrics
{
	public class AffectedFileDto
	{
		public string Key { get; set; } = string.Empty;
		public string Name { get; set; } = string.Empty;
		public string Path { get; set; } = string.Empty;
		public List<ProjectMeasureDto> Measures { get; set; } = [];
	}
}
