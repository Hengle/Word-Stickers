public class ShareManager : ELSingleton<ShareManager>
{
	public int consumedCount;

	public bool firstShowed;

	public int trigerCount;

	public bool readyToDisplay;

	public void Trigger()
	{
		if (!readyToDisplay)
		{
			trigerCount++;
		}
		if ((firstShowed && trigerCount >= ELSingleton<XmlSettings>.Instance.rateItConfig.nextTrigger) || (!firstShowed && trigerCount >= ELSingleton<XmlSettings>.Instance.rateItConfig.firstTrigger))
		{
			readyToDisplay = true;
			trigerCount = 0;
		}
		ELSingleton<ApplicationSettings>.Instance.Save();
	}

	public bool ShouldShow()
	{
		if (HasCoinsReward())
		{
			return readyToDisplay;
		}
		return false;
	}

	public void Show()
	{
		firstShowed = true;
		readyToDisplay = false;
		ELSingleton<ApplicationSettings>.Instance.Save();
	}

	public void Consume()
	{
		consumedCount++;
		ELSingleton<ApplicationSettings>.Instance.Save();
	}

	public bool HasCoinsReward()
	{
		return consumedCount < ELSingleton<XmlSettings>.Instance.shareConfig.coinsNumber;
	}
}
