namespace TFG.Application.Services.OpenProjectIntegration.Dtos
{
	public record OpenProjectRoleDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}
	public record OpenProjectProjectDto
	{
		public int Id { get; set; }
		public string Identifier { get; set; }
		public string Name { get; set; }
		public bool Active { get; set; }
		public bool Public { get; set; }
	}
	public record OpenProjectPrincipalDto
	{
		public int Id { get; set; }
		public string Identifier { get; set; }
		public string Name { get; set; }
		public bool Active { get; set; }
		public bool Public { get; set; }
	}
	public record OpenProjectFormattableDto
	{
		public string Format { get; set; } = "plain";
		public string Raw { get; set; }
	}
}
