using Shared.DTOs.Integrations;

namespace Front.ApiClient.Interfaces
{
	public interface IApiIntegrationsApi
	{
		Task<List<IntegrationDto>> GetIntegrations();
		Task<IntegrationDto> UpdateApiIntegration(ApiConfigurationType type, UpdateApiIntegrationDto apiIntegrationDto);
	}
}
