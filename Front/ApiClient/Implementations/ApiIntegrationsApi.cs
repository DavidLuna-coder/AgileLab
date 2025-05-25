using Front.ApiClient.Interfaces;
using Shared.DTOs.Integrations;

namespace Front.ApiClient.Implementations
{
	public class ApiIntegrationsApi(IApiHttpClient client) : IApiIntegrationsApi
	{
		private const string API_INTEGRATION_ENDPOINT = "api/integrations";

		public Task<List<IntegrationDto>> GetIntegrations()
		{
			return client.GetAsync<List<IntegrationDto>>(API_INTEGRATION_ENDPOINT);
		}

		public Task<IntegrationDto> UpdateApiIntegration(ApiConfigurationType type, UpdateApiIntegrationDto apiIntegrationDto)
		{
			return client.PutAsync<UpdateApiIntegrationDto,IntegrationDto>($"{API_INTEGRATION_ENDPOINT}/{(int)type}", apiIntegrationDto);
		}
	}
}
