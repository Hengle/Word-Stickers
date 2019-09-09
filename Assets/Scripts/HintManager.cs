using System.Collections.Generic;

public class HintManager : ELSingleton<HintManager>
{
	public class Hint
	{
		public LevelHint type;

		public int coins;

		public int amount;

		public bool isAvailable;
	}

	private List<Hint> hints = new List<Hint>();

	public List<Hint> Hints => hints;

	public void Setup()
	{
		for (int i = 0; i < ELSingleton<XmlSettings>.Instance.hintsConfig.Count; i++)
		{
			Hint hint = new Hint();
			hint.type = ELSingleton<XmlSettings>.Instance.hintsConfig[i].type;
			hint.coins = ELSingleton<XmlSettings>.Instance.hintsConfig[i].coins;
			hint.amount = ELSingleton<XmlSettings>.Instance.hintsConfig[i].initial;
			hint.isAvailable = false;
			hints.Add(hint);
		}
	}

	public int GetCoins(LevelHint aHintType)
	{
		for (int i = 0; i < hints.Count; i++)
		{
			if (hints[i].type == aHintType)
			{
				return hints[i].coins;
			}
		}
		return 0;
	}

	public int GetAmount(LevelHint aHintType)
	{
		for (int i = 0; i < hints.Count; i++)
		{
			if (hints[i].type == aHintType)
			{
				return hints[i].amount;
			}
		}
		return 0;
	}

	public void ChangeAmount(LevelHint aHintType, int aAmount)
	{
		for (int i = 0; i < hints.Count; i++)
		{
			if (hints[i].type == aHintType)
			{
				hints[i].amount += aAmount;
				if (hints[i].amount < 0)
				{
					hints[i].amount = 0;
				}
				ELSingleton<ApplicationSettings>.Instance.Save();
			}
		}
	}

	public bool IsHintAvailable(LevelHint aHintType, int aLevelNumber)
	{
		for (int i = 0; i < hints.Count; i++)
		{
			if (hints[i].type == aHintType)
			{
				if (!hints[i].isAvailable && aLevelNumber >= ELSingleton<XmlSettings>.Instance.hintsConfig[i].level - 1)
				{
					hints[i].isAvailable = true;
					ELSingleton<ApplicationSettings>.Instance.Save();
				}
				return hints[i].isAvailable;
			}
		}
		return false;
	}

	public bool CanUseHint()
	{
		int num = 0;
		foreach (Hint hint in Hints)
		{
			if (hint.isAvailable)
			{
				num++;
			}
		}
		if (num == 0)
		{
			return true;
		}
		foreach (Hint hint2 in Hints)
		{
			if (hint2.isAvailable && hint2.amount > 0)
			{
				return true;
			}
		}
		foreach (Hint hint3 in Hints)
		{
			if (hint3.isAvailable && hint3.coins > 0 && hint3.coins <= ELSingleton<CoinsManager>.Instance.Coins)
			{
				return true;
			}
		}
		return false;
	}
}
