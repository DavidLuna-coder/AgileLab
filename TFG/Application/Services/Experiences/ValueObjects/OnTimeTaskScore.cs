namespace TFG.Application.Services.Experiences.ValueObjects
{
	public record OnTimeTaskScore(int OnTimeOpenTasks, int OpenTasks, int MaxOnTimeTasksScore)
	{
		public int CalculateScore()
		{
			if (OpenTasks == 0) return 0;

			double ratio = (double)OnTimeOpenTasks / OpenTasks;
			return (int)(MaxOnTimeTasksScore * ratio);
		}
	}
}
