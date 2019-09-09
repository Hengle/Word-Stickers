public class LevelStats
{
	public int coins;

	public int pointsNormal;

	public int pointsExtra;

	public bool isPerfect;

	public int valid;

	public LevelStats()
	{
		Reset();
	}

	public LevelStats(LevelStats aLevelStats)
	{
		coins = aLevelStats.coins;
		pointsNormal = aLevelStats.pointsNormal;
		pointsExtra = aLevelStats.pointsExtra;
		isPerfect = aLevelStats.isPerfect;
		valid = aLevelStats.valid;
	}

	public void Reset()
	{
		coins = 0;
		pointsNormal = 0;
		pointsExtra = 0;
		isPerfect = true;
		valid = 0;
	}
}
