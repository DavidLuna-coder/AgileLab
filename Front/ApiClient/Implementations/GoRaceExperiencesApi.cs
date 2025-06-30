using Shared.DTOs.Experiences;
using Front.ApiClient.Interfaces;
using Front.ApiClient.Interfaces.GoRaceExperiences;

namespace Front.ApiClient.Implementations.GoRaceExperiences
{
	public class GoRaceExperiencesApi(IApiHttpClient client) : IGoRaceExperiencesApi
	{
		private const string ENDPOINT = "api/experiences";

		public Task<List<GoRaceExperienceDto>> GetAll(string experienceType)
		{
			return client.GetAsync<List<GoRaceExperienceDto>>($"{ENDPOINT}?experienceType={experienceType}");
		}

		public Task<GoRaceExperienceDto> Get(Guid id, string experienceType)
		{
			return client.GetAsync<GoRaceExperienceDto>($"{ENDPOINT}/{id}?experienceType={experienceType}");
		}

		public Task Create(CreateGoRaceExperienceDto dto)
		{
			return client.PostAsync<CreateGoRaceExperienceDto, GoRaceExperienceDto>(ENDPOINT, dto);
		}

		public Task Update(Guid id, UpdateGoRaceExperienceDto dto)
		{
			return client.PutAsync<UpdateGoRaceExperienceDto, GoRaceExperienceDto>($"{ENDPOINT}/{id}", dto);
		}

		public Task Delete(Guid id)
		{
			return client.DeleteAsync($"{ENDPOINT}/{id}");
		}
	}
}
