public class RateUsManager : ELSingleton<RateUsManager>
{
	public bool consumed;

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
		if (!consumed)
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
		consumed = true;
		ELSingleton<ApplicationSettings>.Instance.Save();
	}
}
