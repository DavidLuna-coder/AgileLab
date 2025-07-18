﻿using System.Text.Json;
using TFG.GoRaceClient.Dtos;

namespace TFG.GoRaceClient
{
	public class GoRaceApiClient(IGoRaceHttpClient goRaceHttpCLient) : IGoRaceApiClient
	{
		public async Task<GoRaceDataResponseDto> SendData(List<GoRaceDataRequest> activities)
		{
			HttpResponseMessage response = await goRaceHttpCLient.PostAsync(
				"/v1/api/data",
				activities
			);

			string responseContent = await response.Content.ReadAsStringAsync();

			GoRaceDataResponseDto goRaceResponse = JsonSerializer.Deserialize<GoRaceDataResponseDto>(
				responseContent,
				JsonSerializerOptions.Web
			) ?? throw new JsonException("Failed to deserialize response content.");

			return goRaceResponse;
		}
	}
}
