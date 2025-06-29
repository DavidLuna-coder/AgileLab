using Shared.DTOs.Experiences;

namespace Front.ApiClient.Interfaces
{
	public interface IGoRaceExperiencesApi
	{
		Task<List<GoRaceExperienceDto>> GetAll(string experienceType);
		Task<GoRaceExperienceDto> Get(Guid id, string experienceType);
		Task<GoRaceExperienceDto> Create(CreateGoRaceExperienceDto dto);
		Task<GoRaceExperienceDto> Update(Guid id, UpdateGoRaceExperienceDto dto);
		Task Delete(Guid id);
	}
}
