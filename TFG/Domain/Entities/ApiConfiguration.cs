
using Shared.DTOs.Integrations;

namespace TFG.Domain.Entities
{
	public class ApiConfiguration
	{
		public int Id { get; set; }
		public ApiConfigurationType Type { get; set; }
		public string Name { get; set; }
		public string BaseUrl { get; set; }
		public string Token { get; set; }
	}
}
