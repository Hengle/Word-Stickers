public class PlayerData
{
	private int version;

	private LevelInfo level;

	public static PlayerData Create(string input)
	{
		PlayerData playerData = new PlayerData();
		if (!playerData.Parse(input))
		{
			return null;
		}
		return playerData;
	}

	private bool Parse(string input)
	{
		if (input.Length > 0)
		{
			string[] array = input.Split(';');
			if (array.Length == 4)
			{
				version = int.Parse(array[0]);
				int aWorld = int.Parse(array[1]);
				int aPack = int.Parse(array[2]);
				int aLevel = int.Parse(array[3]);
				if (version == 0)
				{
					level = new LevelInfo(aWorld, aPack, aLevel);
					return true;
				}
			}
			return false;
		}
		return true;
	}

	public void Update()
	{
		LevelInfo firstNotCompleateLevel = ELSingleton<LevelsSettings>.Instance.levelSet.GetFirstNotCompleateLevel();
		if (firstNotCompleateLevel.Compare(level) > 0)
		{
			ELSingleton<LevelsSettings>.Instance.levelSet.SetCurrentLevel(level, forceProgress: true);
		}
		else
		{
			level = firstNotCompleateLevel;
		}
	}

	public string GetData()
	{
		return "0;" + level.currentWorld + ";" + level.currentPack + ";" + level.currentLevel;
	}
}
