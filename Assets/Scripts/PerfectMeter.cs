using UnityEngine;
using UnityEngine.UI;

public class PerfectMeter : MonoBehaviour
{
	public GameObject starLeft;

	public GameObject starRight;

	public GameObject starCenter;

	public Image progressFill;

	public AudioClip fillSound;

	public AudioClip fallSound;

	private Level level;

	private bool isVisible;

	private int lastValid;

	private float fill;

	private void Update()
	{
		if (isVisible)
		{
			if (progressFill.fillAmount < fill)
			{
				progressFill.fillAmount += 0.01f;
			}
			if (progressFill.fillAmount > fill)
			{
				progressFill.fillAmount = fill;
			}
		}
	}

	public void Reset()
	{
		if (base.isActiveAndEnabled)
		{
			starLeft.SetActive(value: false);
			starRight.SetActive(value: false);
			starCenter.SetActive(value: false);
			progressFill.fillAmount = 0f;
			level = null;
			isVisible = false;
			lastValid = 0;
			fill = 0f;
			CancelInvoke("ShowInvoke");
			CancelInvoke("HideInvoke");
			Animator component = base.gameObject.GetComponent<Animator>();
			component.ResetTrigger("StarCenter");
			component.ResetTrigger("StarLeft");
			component.ResetTrigger("StarRight");
			component.ResetTrigger("Hide");
			component.ResetTrigger("Drop");
			component.ResetTrigger("Pop");
			component.Play("Init");
		}
	}

	public void Set(Level aLevel)
	{
		level = aLevel;
	}

	public void Setup()
	{
		if (!isVisible || level == null || level.isCompleted || level.words.Count <= 0 || level.stats.valid <= 0)
		{
			return;
		}
		Animator component = base.gameObject.GetComponent<Animator>();
		if (level.stats.isPerfect)
		{
			if (lastValid != level.stats.valid)
			{
				lastValid = level.stats.valid;
				fill = (float)level.stats.valid * 1f / (float)level.words.Count;
				if (!starRight.activeSelf && fill >= 0.9f)
				{
					starCenter.SetActive(value: true);
					starLeft.SetActive(value: true);
					starRight.SetActive(value: true);
					component.SetTrigger("StarRight");
				}
				else if (!starLeft.activeSelf && fill >= 0.45f)
				{
					starCenter.SetActive(value: true);
					starLeft.SetActive(value: true);
					component.SetTrigger("StarLeft");
				}
				else if (!starCenter.activeSelf)
				{
					starCenter.SetActive(value: true);
					component.SetTrigger("StarCenter");
				}
				else
				{
					component.SetTrigger("Pop");
				}
				ELSingleton<AudioManager>.Instance.PlaySfx(fillSound);
			}
		}
		else if (fill > 0f)
		{
			component.SetTrigger("Drop");
			isVisible = false;
			ELSingleton<AudioManager>.Instance.PlaySfx(fallSound);
		}
	}

	public void Show(float aDelay = 0f)
	{
		Invoke("ShowInvoke", aDelay);
	}

	private void ShowInvoke()
	{
		if (base.isActiveAndEnabled)
		{
			isVisible = true;
			Setup();
		}
	}

	public void Hide(float aDelay = 0f)
	{
		Invoke("HideInvoke", aDelay);
	}

	private void HideInvoke()
	{
		if (base.isActiveAndEnabled)
		{
			base.gameObject.GetComponent<Animator>().SetTrigger("Hide");
		}
	}
}
