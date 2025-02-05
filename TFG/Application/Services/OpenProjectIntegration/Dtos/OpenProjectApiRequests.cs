namespace TFG.Application.Services.OpenProjectIntegration.Dtos
{
	public record OpenProjectCreateProjectDto()
	{
		public required string Name { get; set; }
		public string Identifier
		{
			get => Name.ToLower().Replace(" ", "_");
		}
	}

	public record OpenProjectCreateMembershipsDtos()
	{
		public record OpenProjectCreateMembershipLinks()
		{
			public OpenProjectLink Project { get; set; }
			public OpenProjectLink[] Roles { get; set; }
			public OpenProjectLink Principal { get; set; }
		}
		public OpenProjectCreateMembershipLinks _links { get; set; }
	};

}
