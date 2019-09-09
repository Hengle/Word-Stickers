using UnityEngine;
using UnityEngine.UI;

public class CommonText : MonoBehaviour
{
	public Image fill;

	public Text text;

	public float margin;

	public AudioClip showSound;

	public bool IsShowEnded
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

	public void Reset()
	{
		if (base.isActiveAndEnabled)
		{
			Animator component = base.gameObject.GetComponent<Animator>();
			component.ResetTrigger("Show");
			component.ResetTrigger("Hide");
			component.Play("Init");
			component.enabled = true;
		}
		IsShowEnded = false;
	}

	public void CalculateFill()
	{
		if (fill.isActiveAndEnabled)
		{
			if (Text.Length > 0)
			{
				float num = (text.preferredWidth < text.rectTransform.sizeDelta.x) ? text.preferredWidth : text.rectTransform.sizeDelta.x;
				fill.rectTransform.sizeDelta = new Vector3(num + margin, fill.rectTransform.sizeDelta.y, 0f);
			}
			else
			{
				fill.rectTransform.sizeDelta = new Vector3(0f, fill.rectTransform.sizeDelta.y, 0f);
			}
		}
	}

	public void ResetText()
	{
		Text = string.Empty;
	}

	public void Show(float aDelay = 0f)
	{
		IsShowEnded = false;
		Invoke("ShowInvoke", aDelay);
	}

	private void ShowInvoke()
	{
		if (base.isActiveAndEnabled)
		{
			Animator component = base.gameObject.GetComponent<Animator>();
			component.ResetTrigger("Hide");
			component.SetTrigger("Show");
		}
	}

	private void ShowEnded()
	{
		IsShowEnded = true;
	}

	public void Hide(float aDelay = 0f)
	{
		Invoke("HideInvoke", aDelay);
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

	public void StartAnimation()
	{
		base.gameObject.GetComponent<Animator>().enabled = true;
	}

	public void StopAnimation()
	{
		base.gameObject.GetComponent<Animator>().enabled = false;
	}

	public void ShowSound()
	{
		if ((bool)showSound)
		{
			ELSingleton<AudioManager>.Instance.PlaySfx(showSound);
		}
	}
}
