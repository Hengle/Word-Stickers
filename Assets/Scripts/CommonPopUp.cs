using UnityEngine;

public class CommonPopUp<T> : ELWindow<T> where T : MonoBehaviour
{
	public BlurRenderer_Mobile blur;

	public bool isShowParticles = true;

	protected void Update()
	{
		if ((bool)blur && blur.UpdateBlur)
		{
			blur.UpdateBlur = false;
		}
	}

	public void ShowPopUp(float aDelay = 0f)
	{
		ShowWindow();
		CommonButton[] componentsInChildren = base.gameObject.GetComponentsInChildren<CommonButton>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Reset();
		}
		ParticleSystem[] componentsInChildren2 = base.gameObject.GetComponentsInChildren<ParticleSystem>();
		foreach (ParticleSystem obj in componentsInChildren2)
		{
			obj.Stop();
			obj.Clear();
		}
		RoundParticle[] array = (RoundParticle[])UnityEngine.Object.FindObjectsOfType(typeof(RoundParticle));
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(active: false);
		}
		StickerMainMenu.paused++;
		if ((bool)blur)
		{
			ELSingleton<GameWindow>.Instance.PopUpWillShow();
		}
		Animator component = base.gameObject.GetComponent<Animator>();
		component.ResetTrigger("Show");
		component.ResetTrigger("Hide");
		component.Play("Init");
		component.Update(0f);
		Invoke("ShowPopUpInvoke", aDelay);
	}

	private void ShowPopUpInvoke()
	{
		if (base.isActiveAndEnabled)
		{
			base.gameObject.GetComponent<Animator>().SetTrigger("Show");
			if ((bool)blur)
			{
				blur.UpdateBlur = true;
			}
		}
	}

	protected void ShowPopUpBegan()
	{
		float num = 0.3f;
		CommonButton[] componentsInChildren = base.gameObject.GetComponentsInChildren<CommonButton>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Show(num);
			num += 0.01f;
		}
	}

	protected void ShowPopUpEnded()
	{
	}

	public void HidePopUp(float aDelay = 0f)
	{
		RoundParticle[] array = (RoundParticle[])UnityEngine.Object.FindObjectsOfType(typeof(RoundParticle));
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(active: true);
		}
		CommonButton[] componentsInChildren = base.gameObject.GetComponentsInChildren<CommonButton>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Disable();
		}
		StickerMainMenu.paused--;
		Invoke("HidePopUpInvoke", aDelay);
	}

	private void HidePopUpInvoke()
	{
		if (base.isActiveAndEnabled)
		{
			base.gameObject.GetComponent<Animator>().SetTrigger("Hide");
			if ((bool)blur)
			{
				blur.UpdateBlur = true;
			}
		}
	}

	protected void HidePopUpBegan()
	{
	}

	protected void HidePopUpEnded()
	{
		HideWindow();
		if ((bool)blur)
		{
			ELSingleton<GameWindow>.Instance.PopUpHasHidden();
		}
	}

	protected void ShowParticles()
	{
		if (isShowParticles)
		{
			ParticleSystem[] componentsInChildren = base.gameObject.GetComponentsInChildren<ParticleSystem>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].Play();
			}
		}
	}
}
