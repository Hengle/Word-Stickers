public struct LevelInfo
{
	public int currentWorld;

	public int currentPack;

	public int currentLevel;

	public LevelInfo(int aWorld, int aPack, int aLevel)
	{
		currentWorld = aWorld;
		currentPack = aPack;
		currentLevel = aLevel;
	}

	public static LevelInfo Max(LevelInfo li1, LevelInfo li2)
	{
		if (li1.Compare(li2) <= 0)
		{
			return li1;
		}
		return li2;
	}

	public int Compare(LevelInfo li)
	{
		if (li.currentWorld > currentWorld)
		{
			return 1;
		}
		if (li.currentWorld < currentWorld)
		{
			return -1;
		}
		if (li.currentPack > currentPack)
		{
			return 1;
		}
		if (li.currentPack < currentPack)
		{
			return -1;
		}
		if (li.currentLevel > currentLevel)
		{
			return 1;
		}
		if (li.currentLevel < currentLevel)
		{
			return -1;
		}
		return 0;
	}

	public int ComparePack(LevelInfo li)
	{
		if (li.currentWorld > currentWorld)
		{
			return 1;
		}
		if (li.currentWorld < currentWorld)
		{
			return -1;
		}
		if (li.currentPack > currentPack)
		{
			return 1;
		}
		if (li.currentPack < currentPack)
		{
			return -1;
		}
		return 0;
	}
}
