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

	public record OpenProjectAddMembersToProjectDto(int UserId);
}
