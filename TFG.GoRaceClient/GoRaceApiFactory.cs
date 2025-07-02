namespace TFG.GoRaceClient
{
	public static class GoRaceApiFactory
	{
		public static IGoRaceApiClient Create(string baseUrl, string token)
		{
			var goRaceHttpClient = new GoRaceHttpClient(baseUrl, token);
			return new GoRaceApiClient(goRaceHttpClient);
		}
	}
}
