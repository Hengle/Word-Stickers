public class DailyPuzzleManager : ELSingleton<DailyPuzzleManager>
{
	public long DateLast
	{
		get;
		set;
	} = long.MaxValue;


	public bool IsAvailable
	{
		get;
		private set;
	}

	public void Update()
	{
		long num = (long)(ELSingleton<XmlSettings>.Instance.dailyPuzzleConfig.timeSpan * (float)(ELSingleton<ApplicationSettings>.Instance.DeploymentSettings.isDebug ? 60 : 86400));
		if (DateLast == long.MaxValue)
		{
			DateLast = ELUtils.GetEpochTime() - num;
		}
		IsAvailable = (ELUtils.GetEpochTime() - DateLast > num);
	}

	public void Consume()
	{
		SetupNextTime();
	}

	private void SetupNextTime()
	{
		IsAvailable = false;
		DateLast = ELUtils.GetEpochTime();
		ELSingleton<ApplicationSettings>.Instance.Save();
	}

	public long GetTimeSpan()
	{
		return (long)(ELSingleton<XmlSettings>.Instance.dailyPuzzleConfig.timeSpan * (float)(ELSingleton<ApplicationSettings>.Instance.DeploymentSettings.isDebug ? 60 : 86400)) - ELUtils.GetEpochTime() + DateLast;
	}

	public unsafe string GetTimeSpanString()
	{
		long num = GetTimeSpan() + 1;
		long num2 = num / 3600;
		num %= 3600;
		long num3 = num / 60;
		num %= 60;
		return ((int*)(&num2))->ToString("00") + ":" + ((int*)(&num3))->ToString("00") + ":" + ((int*)(&num))->ToString("00");
	}

	public float GetTimeSpanNormalized()
	{
		long num = (long)(ELSingleton<XmlSettings>.Instance.dailyPuzzleConfig.timeSpan * (float)(ELSingleton<ApplicationSettings>.Instance.DeploymentSettings.isDebug ? 60 : 86400));
		return 1f * (float)GetTimeSpan() / (float)num;
	}
}
