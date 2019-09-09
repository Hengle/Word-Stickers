using UnityEngine;

public class ExtraWordsButton : CommonButton
{
	public GameObject coinTarget;

	public ParticleSystem coinParticle;

	public ParticleSystem fullParticle;

	public AudioClip extraWordCoinSound;

	public AudioClip extraWordFullSound;

	public new void Reset()
	{
		base.Reset();
		CancelInvoke("CheckFullInvoke");
		Animator component = base.gameObject.GetComponent<Animator>();
		component.ResetTrigger("Coin");
		component.Play("Empty");
	}

	public new void Show(float aDelay = 0f, bool aIsAnimation = true)
	{
		base.Show(aDelay, aIsAnimation);
		CheckFull(aDelay);
	}

	public void Coin()
	{
		Animator component = base.gameObject.GetComponent<Animator>();
		component.ResetTrigger("Coin");
		component.SetTrigger("Coin");
	}

	public void CheckFull(float aDelay = 0f)
	{
		Invoke("CheckFullInvoke", aDelay);
	}

	private void CheckFullInvoke()
	{
		if (base.isActiveAndEnabled)
		{
			base.gameObject.GetComponent<Animator>().SetBool("IsFull", ELSingleton<ExtraWordsManager>.Instance.IsFull);
		}
	}

	private void CoinSound()
	{
		ELSingleton<AudioManager>.Instance.PlaySfx(extraWordCoinSound);
	}

	private void FullSound()
	{
		ELSingleton<AudioManager>.Instance.PlaySfx(extraWordFullSound);
	}

	private void CoinParticle()
	{
		coinParticle.Play();
	}

	public void FullParticleStart()
	{
		fullParticle.Play();
	}

	public void FullParticleStop()
	{
		fullParticle.Stop();
		fullParticle.Clear();
	}
}
