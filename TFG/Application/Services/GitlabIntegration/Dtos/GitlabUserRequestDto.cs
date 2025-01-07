namespace TFG.Application.Services.GitlabIntegration.Dtos
{
	public struct GitlabUserRequestDto
	{
		public string Email { get; set; }
		public string Username { get; set; }
		public string Name { get; set; }
		public string Password { get; set; }
	}
}
