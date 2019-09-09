using Assets.SimpleAndroidNotifications;
using System;
using UnityEngine;

public class NotificationsManager : ELSingleton<NotificationsManager>
{
	private const int EMOJI_NUM = 12;

	public void RegisterNotifications(int aPackIndex, int aLevelIndex)
	{
		int pack = ELSingleton<XmlSettings>.Instance.notificationsConfig.pack;
		int level = ELSingleton<XmlSettings>.Instance.notificationsConfig.level;
		if (aPackIndex != pack - 1 || aLevelIndex < level - 1)
		{
		}
	}

	public void CancelNotifications()
	{
		NotificationManager.CancelAll();
	}

	public void ScheduleNotifications()
	{
		CancelNotifications();
		ScheduleNotification(0, (!ELSingleton<DailyPuzzleManager>.Instance.IsAvailable) ? DateTime.Now.AddSeconds(ELSingleton<DailyPuzzleManager>.Instance.GetTimeSpan()) : (ELSingleton<ApplicationSettings>.Instance.DeploymentSettings.isDebug ? DateTime.Now.AddMinutes(1.0) : DateTime.Now.AddDays(1.0)), "Hey there! Daily Puzzle is ready!");
		for (int i = 2; i <= 7; i++)
		{
			ScheduleNotification(i - 1, ELSingleton<ApplicationSettings>.Instance.DeploymentSettings.isDebug ? DateTime.Now.AddMinutes(i) : DateTime.Now.AddDays(i), "Hey there! Daily Puzzle is ready!");
		}
	}

	private void ScheduleNotification(int aIndex, DateTime aDateTime, string aMessage)
	{
		if (aMessage != null && !string.Equals(aMessage, string.Empty))
		{
			aMessage = $"{aMessage} {ELSingleton<LanguageSettings>.Instance.DecodeEncodedNonAsciiCharacters(ELSingleton<LanguageSettings>.Instance.GetString(10 + UnityEngine.Random.Range(0, 12)))}";
			NotificationManager.SendCustom(new NotificationParams
			{
				Id = aIndex,
				Delay = aDateTime - DateTime.Now,
				Title = "Word Stickers!",
				Message = aMessage,
				Ticker = "Ticker",
				Sound = true,
				Vibrate = true,
				Light = true,
				SmallIcon = NotificationIcon.Heart,
				SmallIconColor = new Color(0.804f, 0.18f, 0.204f),
				LargeIcon = "app_icon"
			});
		}
	}
}
