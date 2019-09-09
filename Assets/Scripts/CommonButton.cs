using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CommonButton : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IPointerClickHandler
{
	public Text text;

	public bool isClickable;

	public UnityEvent OnClick;

	public AudioClip clickSound;

	public bool IsEnabled
	{
		get;
		private set;
	}

	public bool IsShown
	{
		get;
		private set;
	}

	public string Text
	{
		get
		{
			return text.text;
		}
		set
		{
			text.text = value;
		}
	}

	private void Awake()
	{
		IsEnabled = true;
		IsShown = false;
	}

	private void Update()
	{
	}

	public void Reset()
	{
		if (base.isActiveAndEnabled)
		{
			CancelInvoke("EnableInvoke");
			CancelInvoke("ShowInvokeWithAnimation");
			CancelInvoke("ShowInvokeNoAnimation");
			CancelInvoke("HideInvoke");
			CancelInvoke("PopInvoke");
			Animator component = base.gameObject.GetComponent<Animator>();
			component.ResetTrigger("Show");
			component.ResetTrigger("Hide");
			component.ResetTrigger("Clicked");
			component.ResetTrigger("Unclicked");
			component.ResetTrigger("Pop");
			component.Play("Init");
			component.enabled = true;
			IsEnabled = true;
			IsShown = false;
		}
	}

	public void Enable(float aDelay = 0f)
	{
		Invoke("EnableInvoke", aDelay);
	}

	private void EnableInvoke()
	{
		if (base.isActiveAndEnabled)
		{
			IsEnabled = true;
		}
	}

	public void Disable()
	{
		CancelInvoke("EnableInvoke");
		IsEnabled = false;
	}

	public void Show(float aDelay = 0f, bool aIsAnimation = true)
	{
		if (aIsAnimation)
		{
			Invoke("ShowInvokeWithAnimation", aDelay);
		}
		else
		{
			Invoke("ShowInvokeNoAnimation", aDelay);
		}
	}

	private void ShowInvokeWithAnimation()
	{
		if (base.isActiveAndEnabled)
		{
			Animator component = base.gameObject.GetComponent<Animator>();
			component.ResetTrigger("Hide");
			component.SetTrigger("Show");
		}
	}

	private void ShowInvokeNoAnimation()
	{
		if (base.isActiveAndEnabled)
		{
			base.gameObject.GetComponent<Animator>().Play("Idle");
		}
	}

	public void ShowEnded()
	{
		IsShown = true;
	}

	public void Hide(float aDelay = 0f)
	{
		Invoke("HideInvoke", aDelay);
		IsShown = false;
	}

	private void HideInvoke()
	{
		if (base.isActiveAndEnabled)
		{
			Animator component = base.gameObject.GetComponent<Animator>();
			component.ResetTrigger("Show");
			component.SetTrigger("Hide");
		}
	}

	public void Pop(bool aIsWithParticles = false)
	{
		Animator component = base.gameObject.GetComponent<Animator>();
		component.SetInteger("PopIndex", UnityEngine.Random.Range(0, 3));
		component.SetTrigger("Pop");
		if (aIsWithParticles)
		{
			ParticleSystem[] componentsInChildren = base.gameObject.GetComponentsInChildren<ParticleSystem>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].Play();
			}
		}
	}

	public void Click()
	{
		Animator component = base.gameObject.GetComponent<Animator>();
		component.ResetTrigger("Unclicked");
		component.SetTrigger("Clicked");
	}

	public void Unclick()
	{
		Animator component = base.gameObject.GetComponent<Animator>();
		component.ResetTrigger("Clicked");
		component.SetTrigger("Unclicked");
	}

	public void OnPointerDown(PointerEventData aEventData)
	{
		if (isClickable && IsEnabled)
		{
			Click();
		}
	}

	public void OnPointerUp(PointerEventData aEventData)
	{
		if (isClickable && IsEnabled)
		{
			Unclick();
		}
	}

	public virtual void OnPointerClick(PointerEventData aEventData)
	{
		if (isClickable && IsEnabled)
		{
			if ((bool)clickSound)
			{
				ELSingleton<AudioManager>.Instance.PlaySfx(clickSound);
			}
			OnClick.Invoke();
		}
	}

	public void StartAnimation()
	{
		base.gameObject.GetComponent<Animator>().enabled = true;
	}

	public void StopAnimation()
	{
		base.gameObject.GetComponent<Animator>().enabled = false;
	}
}
