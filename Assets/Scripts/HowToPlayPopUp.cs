using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToPlayPopUp : CommonPopUp<HowToPlayPopUp>
{
	public CommonButton exitButton;

	public GameObject[] pages;

	public int pageIndex;

	private new void Update()
	{
		base.Update();
		if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
		{
			ExitButton();
		}
	}

	public new void ShowPopUp(float aDelay = 0f)
	{
		base.ShowPopUp(aDelay);
		exitButton.Enable();
		pageIndex = 0;
		GameObject[] array = pages;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(value: false);
		}
		ELSingleton<AnalyticsManager>.Instance.TutorialStart("HowToPlayPopUp", new Dictionary<string, object>
		{
			{
				"screen",
				ELSingleton<MenuWindow>.Instance.isActiveAndEnabled ? "MenuWindow" : "GameWindow"
			}
		});
	}

	private new void ShowPopUpBegan()
	{
		base.ShowPopUpBegan();
		pages[0].SetActive(value: true);
		pages[0].GetComponent<CanvasGroup>().alpha = 1f;
		Component[] componentsInChildren = base.gameObject.GetComponentsInChildren<CommonButton>();
		componentsInChildren = componentsInChildren;
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			((CommonButton)componentsInChildren[i]).Show(0f, aIsAnimation: false);
		}
		ELSingleton<AnalyticsManager>.Instance.TutorialStep(0, "HowToPlayPopUp", new Dictionary<string, object>
		{
			{
				"screen",
				ELSingleton<MenuWindow>.Instance.isActiveAndEnabled ? "MenuWindow" : "GameWindow"
			}
		});
	}

	public void ExitButton()
	{
		exitButton.Disable();
		HidePopUp();
		ELSingleton<GameWindow>.Instance.HowToPlayExitButton();
		ELSingleton<AnalyticsManager>.Instance.TutorialSkip("HowToPlayPopUp", new Dictionary<string, object>
		{
			{
				"screen",
				ELSingleton<MenuWindow>.Instance.isActiveAndEnabled ? "MenuWindow" : "GameWindow"
			}
		});
	}

	public void NextButton()
	{
		pageIndex++;
		if (pageIndex >= pages.Length)
		{
			exitButton.Disable();
			HidePopUp();
			ELSingleton<GameWindow>.Instance.HowToPlayExitButton();
		}
		else
		{
			StartCoroutine(TurnOff(pages[pageIndex - 1], 0.25f));
			StartCoroutine(TurnOn(pages[pageIndex], 0.25f, 0.25f));
			ELSingleton<AnalyticsManager>.Instance.TutorialStep(pageIndex, "HowToPlayPopUp", new Dictionary<string, object>
			{
				{
					"screen",
					ELSingleton<MenuWindow>.Instance.isActiveAndEnabled ? "MenuWindow" : "GameWindow"
				}
			});
		}
	}

	public IEnumerator TurnOff(GameObject go, float time)
	{
		CanvasGroup cg = go.GetComponent<CanvasGroup>();
		float elapsedTime = 0f;
		while (elapsedTime < time)
		{
			cg.alpha = 1f - elapsedTime / time;
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		go.SetActive(value: false);
	}

	public IEnumerator TurnOn(GameObject go, float time1, float time2)
	{
		float elapsedTime2 = 0f;
		while (elapsedTime2 < time1)
		{
			elapsedTime2 += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		elapsedTime2 = 0f;
		CanvasGroup cg = go.GetComponent<CanvasGroup>();
		go.SetActive(value: true);
		while (elapsedTime2 < time2)
		{
			cg.alpha = elapsedTime2 / time2;
			elapsedTime2 += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		cg.alpha = 1f;
		Component[] componentsInChildren = base.gameObject.GetComponentsInChildren<CommonButton>();
		componentsInChildren = componentsInChildren;
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			((CommonButton)componentsInChildren[i]).Show();
		}
	}
}
