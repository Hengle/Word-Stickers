public class PointsManager : ELSingleton<PointsManager>
{
	private int points;

	public int Points
	{
		get
		{
			return points;
		}
		set
		{
			points = value;
		}
	}

	public void AddPoints(int aPoints)
	{
		points += aPoints;
		if (points < 0)
		{
			points = 0;
		}
		ELSingleton<ApplicationSettings>.Instance.Save();
	}
}
