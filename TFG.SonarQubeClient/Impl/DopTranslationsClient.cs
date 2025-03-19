using System.Text.Json;
using TFG.SonarQubeClient.Models;

namespace TFG.SonarQubeClient.Impl
{
	public class DopTranslationsClient(ISonarHttpClient client) : IDopTranslationsClient
	{
		private readonly JsonSerializerOptions _serializerOptions = new(JsonSerializerDefaults.Web);
		public async Task<BoundedProject> BoundProjectAsync(ProjectBinding projectBinding)
		{
			var response = await client.PostAsync("dop-translation/bound-projects", projectBinding, "v2");
			string responseBody = await response.Content.ReadAsStringAsync();
			BoundedProject boundedProject = JsonSerializer.Deserialize<BoundedProject>(responseBody, _serializerOptions) ?? throw new HttpRequestException("Failed project deserialization");
			return boundedProject;
		}

		public async Task<DopSettingsResponse> GetDopSettingsAsync()
		{
			var response = await client.GetAsync("dop-translation/dop-settings", "v2");
			string responseBody = await response.Content.ReadAsStringAsync();
			DopSettingsResponse dopSettings = JsonSerializer.Deserialize<DopSettingsResponse>(responseBody, _serializerOptions) ?? throw new HttpRequestException("Failed settings deserialization");
			return dopSettings;
		}
	}
}
