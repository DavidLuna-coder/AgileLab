namespace TFG.SonarQubeClient.Models.Metrics
{
	public class SonarComponent
	{
		public string Key { get; set; } = string.Empty;
		public List<SonarMeasure> Measures { get; set; } = new();
	}
}
