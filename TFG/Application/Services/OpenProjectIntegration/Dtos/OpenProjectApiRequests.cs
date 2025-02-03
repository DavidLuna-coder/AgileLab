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
			public OpenProjectCreateMembershipsLinksProperties Properties { get; set; }
		}
		public record OpenProjectCreateMembershipsLinksProperties
		{
			public int ProjectId { get; set; }
			public OpenProjectRoleDto[] Roles { get; set; }
			public OpenProjectPrincipalDto Principal { get; set; }
		}
		public record OpenProjectCreateMembershipMeta 
		{ 
			public bool SendNotification { get; set; } = false;
		}
		public OpenProjectCreateMembershipLinks _Links { get; set; }
		public OpenProjectCreateMembershipMeta _Meta { get; set; }
	};

}
