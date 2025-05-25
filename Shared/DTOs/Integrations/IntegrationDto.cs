namespace Shared.DTOs.Integrations
{
	public class IntegrationDto
	{
		public string Name { get; set; }
		public ApiConfigurationType Type { get; set; }
		public string BaseUrl { get; set; }
		public string Token { get; set; }
	}
}
