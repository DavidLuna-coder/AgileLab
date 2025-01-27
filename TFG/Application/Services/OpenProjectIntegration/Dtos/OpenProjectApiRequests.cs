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
		public record OpenProjectCreateMembershipLinks
		{
			public int ProjectId { get; set; }
			public OpenProjectRoleDto[] Roles{ get; set; }

		}
		public bool SendNotification { get; set; } = false;

	};

}
