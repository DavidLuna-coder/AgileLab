using TFG.GoRaceClient.Dtos;

namespace TFG.GoRaceClient
{
	public interface IGoRaceApiClient
	{
		Task<GoRaceDataResponseDto> SendData(List<GoRaceDataRequest> activities);
	}
}
