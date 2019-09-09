using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Ladybug : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
{
	private const int IDLE_ANIMATION_NUM = 4;

	private const int DELAY_TIME_MIN = 15;

	private const int DELAY_TIME_MAX = 60;

	public GameObject pod;

	public Image bug;

	public Sprite[] bugSprite;

	private bool isActive;

	private int bugSpriteIndex;

	private bool isAnimating;

	private void Update()
	{
		Animator component = base.gameObject.GetComponent<Animator>();
		if (isActive && isAnimating && component.enabled)
		{
			bug.sprite = bugSprite[bugSpriteIndex / 2 % bugSprite.Length];
			bugSpriteIndex++;
		}
	}

	public void Reset()
	{
		isActive = false;
		bugSpriteIndex = 0;
		isAnimating = true;
		if (base.isActiveAndEnabled)
		{
			Animator component = base.gameObject.GetComponent<Animator>();
			component.Play("Init");
			component.enabled = true;
		}
		CancelInvoke("ShowInvoke");
	}

	public void Show()
	{
		Reset();
		Invoke("ShowInvoke", UnityEngine.Random.Range(15, 60));
	}

	private void ShowInvoke()
	{
		if (base.isActiveAndEnabled)
		{
			isActive = true;
			base.gameObject.GetComponent<Animator>().Play($"Show{Random.Range(0, 4)}");
		}
	}

	public void Run()
	{
		isAnimating = true;
	}

	public void Hold()
	{
		isAnimating = false;
	}

	public void StartAnimation()
	{
		base.gameObject.GetComponent<Animator>().enabled = true;
	}

	public void StopAnimation()
	{
		base.gameObject.GetComponent<Animator>().enabled = false;
	}

	public void OnPointerDown(PointerEventData aEventData)
	{
		Reset();
		int ladybug = ELSingleton<XmlSettings>.Instance.coinsConfig.ladybug;
		if (ladybug > 0)
		{
			ELSingleton<CoinsManager>.Instance.AddCoins(ladybug);
			ELSingleton<GameWindow>.Instance.coinPod.ReleaseCoinsLinearly(ladybug, pod.transform.position, ELSingleton<GameWindow>.Instance.shopButton.coinTarget.transform.position, 2f, 0.5f, 0.5f, 0f, aIsFullUpdate: false, aIsWithParticles: true);
		}
	}
}
