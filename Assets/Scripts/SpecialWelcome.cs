using UnityEngine;
using UnityEngine.UI;

public class SpecialWelcome : MonoBehaviour
{
	public GameObject bounsRound;

	public GameObject dailyPuzzle;

	public Text messageText;

	public CommonButton playButton;

	public AudioClip soundShow;

	public void Reset()
	{
		if (base.isActiveAndEnabled)
		{
			Animator component = base.gameObject.GetComponent<Animator>();
			component.ResetTrigger("Show");
			component.ResetTrigger("Hide");
			component.Play("Init");
			CommonButton[] componentsInChildren = base.gameObject.GetComponentsInChildren<CommonButton>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].Reset();
			}
		}
	}

	public void Show(LevelType aLevelType, float aDelay = 0f)
	{
		switch (aLevelType)
		{
		case LevelType.BonusRound:
			bounsRound.SetActive(value: true);
			dailyPuzzle.SetActive(value: false);
			messageText.text = "Welcome to <color=#39A2A6>Bonus Round</color>.\nMake the <color=#39A2A6>longest</color> word you can. \nEarn <color=#39A2A6>Coin</color> for each letter used.";
			break;
		case LevelType.DailyPuzzle:
			bounsRound.SetActive(value: false);
			dailyPuzzle.SetActive(value: true);
			messageText.text = "Welcome to <color=#39A2A6>Daily Puzzle</color>.\nWorkout your brain with\n<color=#39A2A6>extra level</color> each day.\nEarn <color=#39A2A6>Coin</color> for each letter used.";
			break;
		}
		Invoke("ShowInvoke", aDelay);
	}

	private void ShowInvoke()
	{
		if (base.isActiveAndEnabled)
		{
			base.gameObject.GetComponent<Animator>().SetTrigger("Show");
		}
	}

	private void ShowPopUpEnded()
	{
		CommonButton[] componentsInChildren = base.gameObject.GetComponentsInChildren<CommonButton>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Show();
		}
	}

	public void Hide(float aDelay = 0f)
	{
		Invoke("HideInvoke", aDelay);
		CommonButton[] componentsInChildren = base.gameObject.GetComponentsInChildren<CommonButton>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Disable();
		}
	}

	private void HideInvoke()
	{
		if (base.isActiveAndEnabled)
		{
			base.gameObject.GetComponent<Animator>().SetTrigger("Hide");
		}
	}

	private void HideEnded()
	{
	}

	private void ShowSound()
	{
		ELSingleton<AudioManager>.Instance.PlaySfx(soundShow);
	}
}
