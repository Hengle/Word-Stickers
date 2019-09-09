using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TutorialWindow : ELWindow<TutorialWindow>, IPointerDownHandler, IEventSystemHandler
{
	public GameObject hand;

	public GameObject message;

	public Text messageText;

	private TutorialType tutorialType;

	public bool isPointerDown;

	private void Update()
	{
		isPointerDown = false;
	}

	public void Show(Tutorial aTutorial, Vector3 aPosition, float aDelay = 0f)
	{
		ShowWindow();
		hand.transform.localPosition = new Vector3(aPosition.x + aTutorial.hand.x, aPosition.y + aTutorial.hand.y, 0f);
		message.transform.localPosition = new Vector3(aPosition.x + aTutorial.message.x, aPosition.y + aTutorial.message.y, 0f);
		messageText.text = ((aTutorial.type == TutorialType.GoodStart) ? "<color=#FF4B00>Good Start</color> will show you first letter of the word." : ((aTutorial.type == TutorialType.StartAndFinish) ? "<color=#FF4B00>Start & Finish</color> will show you first and last letter of the word." : ((aTutorial.type == TutorialType.Expose) ? "<color=#FF4B00>Expose</color> will show you whole word and its shape." : ((aTutorial.type == TutorialType.Tip) ? "<color=#FF4B00>Tip</color> will show you whole word without location and shape." : ((aTutorial.type == TutorialType.Perfect) ? "Make no mistakes to earn Perfect Bonus." : ((aTutorial.type == TutorialType.Dictionary) ? "You can tap on a word to see its definition." : ((aTutorial.type == TutorialType.Shop) ? "Stuck? You can always buy more coins or  hints." : ((aTutorial.type == TutorialType.ExtraWord) ? "Make extra words to earn extra coins." : ""))))))));
		tutorialType = aTutorial.type;
		isPointerDown = false;
		Animator component = base.gameObject.GetComponent<Animator>();
		component.ResetTrigger("Hide");
		component.ResetTrigger("HideNow");
		component.SetBool("IsPoint", tutorialType == TutorialType.Perfect);
		Invoke("ShowInvoke", aTutorial.delay);
	}

	private void ShowInvoke()
	{
		base.gameObject.GetComponent<Animator>().SetTrigger("Show");
	}

	public void Hide()
	{
		Animator component = base.gameObject.GetComponent<Animator>();
		component.SetTrigger("Hide");
		component.ResetTrigger("Show");
	}

	public void HideNow()
	{
		Animator component = base.gameObject.GetComponent<Animator>();
		component.SetTrigger("HideNow");
		component.ResetTrigger("Show");
	}

	public void HideEnded()
	{
		HideWindow();
		ELSingleton<TutorialManager>.Instance.Reset();
	}

	public void Click()
	{
		if (tutorialType == TutorialType.GoodStart)
		{
			ELSingleton<GameWindow>.Instance.goodStartButton.Click();
		}
		else if (tutorialType == TutorialType.StartAndFinish)
		{
			ELSingleton<GameWindow>.Instance.startAndFinishButton.Click();
		}
		else if (tutorialType == TutorialType.Expose)
		{
			ELSingleton<GameWindow>.Instance.exposeButton.Click();
		}
		else if (tutorialType == TutorialType.Tip)
		{
			ELSingleton<GameWindow>.Instance.tipButton.Click();
		}
		else if (tutorialType == TutorialType.Dictionary)
		{
			ELSingleton<GameWindow>.Instance.board.wordText.Click();
		}
		else if (tutorialType == TutorialType.Shop)
		{
			ELSingleton<GameWindow>.Instance.shopButton.Click();
		}
		else if (tutorialType == TutorialType.ExtraWord)
		{
			ELSingleton<GameWindow>.Instance.extraWordsButton.Click();
		}
	}

	public void Unclick()
	{
		if (tutorialType == TutorialType.GoodStart)
		{
			ELSingleton<GameWindow>.Instance.goodStartButton.Unclick();
		}
		else if (tutorialType == TutorialType.StartAndFinish)
		{
			ELSingleton<GameWindow>.Instance.startAndFinishButton.Unclick();
		}
		else if (tutorialType == TutorialType.Expose)
		{
			ELSingleton<GameWindow>.Instance.exposeButton.Unclick();
		}
		else if (tutorialType == TutorialType.Tip)
		{
			ELSingleton<GameWindow>.Instance.tipButton.Unclick();
		}
		else if (tutorialType == TutorialType.Dictionary)
		{
			ELSingleton<GameWindow>.Instance.board.wordText.Unclick();
		}
		else if (tutorialType == TutorialType.Shop)
		{
			ELSingleton<GameWindow>.Instance.shopButton.Unclick();
		}
		else if (tutorialType == TutorialType.ExtraWord)
		{
			ELSingleton<GameWindow>.Instance.extraWordsButton.Unclick();
		}
	}

	public void OnPointerDown(PointerEventData aEventData)
	{
		isPointerDown = true;
		HideNow();
	}
}
