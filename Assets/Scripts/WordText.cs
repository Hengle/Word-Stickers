using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class WordText : CommonText, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IPointerClickHandler
{
	public UnityEvent OnClick;

	public bool IsDictionary
	{
		get;
		private set;
	}

	public new void Reset()
	{
		base.Reset();
		IsDictionary = false;
		Animator component = base.gameObject.GetComponent<Animator>();
		component.ResetTrigger("Clicked");
		component.ResetTrigger("Unclicked");
	}

	private new void Hide(float aDelay = 0f)
	{
		base.Hide(aDelay);
		IsDictionary = false;
	}

	public void Idle()
	{
		base.gameObject.GetComponent<Animator>().Play("Idle");
		IsDictionary = false;
	}

	public void Valid()
	{
		base.gameObject.GetComponent<Animator>().Play("Valid", -1, 0f);
		IsDictionary = true;
	}

	public void Invalid()
	{
		base.gameObject.GetComponent<Animator>().Play("Invalid");
		IsDictionary = false;
	}

	public void ExtraWord()
	{
		base.gameObject.GetComponent<Animator>().Play("ExtraWord");
		IsDictionary = true;
	}

	public void Repeat()
	{
		base.gameObject.GetComponent<Animator>().Play("Repeat");
		IsDictionary = true;
	}

	public void Click()
	{
		if (IsDictionary)
		{
			Animator component = base.gameObject.GetComponent<Animator>();
			component.ResetTrigger("Unclicked");
			component.SetTrigger("Clicked");
		}
	}

	public void Unclick()
	{
		if (IsDictionary)
		{
			Animator component = base.gameObject.GetComponent<Animator>();
			component.ResetTrigger("Clicked");
			component.SetTrigger("Unclicked");
		}
	}

	public void OnPointerDown(PointerEventData aEventData)
	{
		Click();
	}

	public void OnPointerUp(PointerEventData aEventData)
	{
		Unclick();
	}

	public virtual void OnPointerClick(PointerEventData aEventData)
	{
		if (IsDictionary)
		{
			OnClick.Invoke();
		}
	}
}
