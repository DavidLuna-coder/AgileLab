using Shared.DTOs.Experiences;

namespace Front.ApiClient.Interfaces.GoRaceExperiences
{
	public interface IGoRaceExperiencesApi
	{
		Task<List<GoRaceExperienceDto>> GetAll(string experienceType);
		Task<GoRaceExperienceDto> Get(Guid id, string experienceType);
		Task Create(CreateGoRaceExperienceDto dto);
		Task Update(Guid id, UpdateGoRaceExperienceDto dto);
		Task Delete(Guid id);
	}
}
