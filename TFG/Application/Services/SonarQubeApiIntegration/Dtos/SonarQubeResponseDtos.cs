namespace TFG.Application.Services.Dtos
{
	public record SonarQubeCreateProjectResponseDto
	{
		public string ProjectId { get; set; }
		public string BindingId { get; set; }
	}

	public record SonarQubeGetDopSettingsDto
	{
		public List<SonarQubeDopSettingsDto> DopSettings { get; set; } = [];
	}
	public record SonarQubeDopSettingsDto()
	{
		public string Id { get; set; }
		public string Type { get; set; }
		public string Key { get; set; }
		public string Url { get; set; }
		public string AppId { get; set; }
	}

	public record SonarQubeDeleteProjectDto()
	{
		public string Project { get; set; } //project key
	}
}
