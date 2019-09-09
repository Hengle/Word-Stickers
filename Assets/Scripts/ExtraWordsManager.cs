public class ExtraWordsManager : ELSingleton<ExtraWordsManager>
{
	private int index;

	private int wordsCurrent;

	private int wordsMax;

	private bool isFull;

	public int Index
	{
		get
		{
			return index;
		}
		set
		{
			index = value;
		}
	}

	public int WordsCurrent
	{
		get
		{
			return wordsCurrent;
		}
		set
		{
			wordsCurrent = value;
		}
	}

	public int WordsMax => wordsMax;

	public int Coins => ELSingleton<XmlSettings>.Instance.extraWordsConfigs[index].coins;

	public bool IsFull
	{
		get
		{
			return isFull;
		}
		set
		{
			isFull = value;
		}
	}

	private void Awake()
	{
		index = 0;
		wordsCurrent = 0;
		wordsMax = 0;
		isFull = false;
	}

	public void Check()
	{
		if (index < 0)
		{
			index = 0;
		}
		else if (index >= ELSingleton<XmlSettings>.Instance.extraWordsConfigs.Count)
		{
			index = ELSingleton<XmlSettings>.Instance.extraWordsConfigs.Count - 1;
		}
		wordsMax = ELSingleton<XmlSettings>.Instance.extraWordsConfigs[index].words;
		if (wordsCurrent >= wordsMax)
		{
			wordsCurrent = wordsMax;
			isFull = true;
		}
	}

	public void AddWords(int aWords)
	{
		wordsCurrent += aWords;
		Check();
		ELSingleton<ApplicationSettings>.Instance.Save();
	}

	public void Collect()
	{
		if (isFull)
		{
			if (Coins > 0)
			{
				ELSingleton<CoinsManager>.Instance.AddCoins(Coins);
				ELSingleton<GameWindow>.Instance.coinPod.ReleaseCoinsRadial(Coins, ELSingleton<GameWindow>.Instance.extraWordsButton.transform.position, ELSingleton<GameWindow>.Instance.shopButton.coinTarget.transform.position, 1f, 0.8f, 0.8f, 0.5f, aIsFullUpdate: false, aIsWithParticles: true);
			}
			index++;
			wordsCurrent = 0;
			wordsMax = 0;
			isFull = false;
			Check();
			ELSingleton<GameWindow>.Instance.extraWordsButton.CheckFull();
			ELSingleton<ApplicationSettings>.Instance.Save();
		}
	}
}
