using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ApplicationManager : ELSingleton<ApplicationManager>
{
	private bool isPaused;

	public bool IsPaused => isPaused;

	private void Awake()
	{
		Application.targetFrameRate = 60;
		isPaused = false;
	}

	private void Start()
	{
		EventSystem.current.pixelDragThreshold = 64;
		ELSingleton<MenuWindow>.Instance.InitWindow();
		StartCoroutine(StartCoroutineTask());
	}

	private void OnApplicationFocus(bool hasFocus)
	{
		isPaused = !hasFocus;
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		isPaused = pauseStatus;
		if (isPaused)
		{
			if (ELSingleton<ApplicationSettings>.Instance.IsLoadCompleted)
			{
				ELSingleton<NotificationsManager>.Instance.ScheduleNotifications();
			}
			ELSingleton<ApplicationSettings>.Instance.Save(aIsForceSave: true);
		}
	}

	private void OnApplicationQuit()
	{
		ELSingleton<ApplicationSettings>.Instance.Save(aIsForceSave: true);
		bool isLoadCompleted = ELSingleton<ApplicationSettings>.Instance.IsLoadCompleted;
	}

	private IEnumerator StartCoroutineTask()
	{
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		ELSingleton<ApplicationSettings>.Instance.LoadSettings(this, "LoadSettingsCompleted");
	}

	private void LoadSettingsCompleted()
	{
		if (ELSingleton<ApplicationSettings>.Instance.applicationData.session == 0)
		{
			if (!ELSingleton<GDPRManager>.Instance.isGDPRAction())
			{
				ShowGameAfterLoading();
			}
		}
		else
		{
			FirstTimeShowMenu();
		}
	}

	private void FirstTimeShowMenu()
	{
		ELSingleton<MenuWindow>.Instance.ShowWindow();
		ELSingleton<MenuWindow>.Instance.PopUps();
		ELSingleton<MusicManager>.Instance.PlayMenu();
		Invoke("HideLoadingInvoke", 0f);
	}

	private void HideLoadingInvoke()
	{
		ELSingleton<LoadingWindow>.Instance.HideWindow();
	}

	public void ShowPackAfterMenu()
	{
		ELSingleton<MenuWindow>.Instance.HideWindow();
		ELSingleton<PackWindow>.Instance.ShowWindow(new LevelInfo(0, 0, 0));
	}

	public void ShowMenuAfterPack()
	{
		ELSingleton<PackWindow>.Instance.HideWindow();
		ELSingleton<MenuWindow>.Instance.ShowWindow();
	}

	public void ShowMenuAfterGame()
	{
		ELSingleton<GameWindow>.Instance.HideWindow();
		ELSingleton<MenuWindow>.Instance.ShowWindow();
		ELSingleton<MusicManager>.Instance.PlayMenu();
	}

	public void ShowPackAfterGame()
	{
		ELSingleton<GameWindow>.Instance.HideWindow();
		ELSingleton<PackWindow>.Instance.ShowWindow(ELSingleton<LevelsSettings>.Instance.levelSet.GetCurrentLevelInfo());
		ELSingleton<MusicManager>.Instance.PlayMenu();
	}

	public void ShowPackCompleteAfterGame()
	{
		ELSingleton<GameWindow>.Instance.HideWindow();
		ELSingleton<PackWindow>.Instance.ShowWindow(ELSingleton<LevelsSettings>.Instance.levelSet.GetCurrentLevelInfo(), PackWindow.AnimationState.ANIMATION_STATE_PACK_WAIT);
		ELSingleton<MusicManager>.Instance.PlayMenu();
	}

	public void ShowGameAfterPack()
	{
		ELSingleton<PackWindow>.Instance.HideWindow();
		ELSingleton<GameWindow>.Instance.ShowWindow();
	}

	public void ShowGameAfterMenu()
	{
		ELSingleton<MenuWindow>.Instance.HideWindow();
		ELSingleton<GameWindow>.Instance.ShowWindow();
	}

	public void ShowGameAfterLoading()
	{
		ELSingleton<LoadingWindow>.Instance.HideWindow();
		ELSingleton<GameWindow>.Instance.ShowWindow();
	}
}
