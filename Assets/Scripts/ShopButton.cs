using UnityEngine;

public class ShopButton : CommonButton
{
	private const int COINS_TARGET_FACTOR = 10;

	private int coinsCurrent;

	private int coinsTarget;

	public GameObject coinTarget;

	public GameObject shineParticle;

	private void Awake()
	{
		coinsCurrent = 0;
		coinsTarget = 0;
	}

	private void Update()
	{
		if (coinsCurrent < coinsTarget)
		{
			if (coinsTarget - coinsCurrent <= 10)
			{
				coinsCurrent++;
			}
			else
			{
				coinsCurrent += (coinsTarget - coinsCurrent) / 10;
			}
			base.Text = coinsCurrent.ToString();
		}
		else if (coinsCurrent > coinsTarget)
		{
			if (coinsCurrent - coinsTarget <= 10)
			{
				coinsCurrent--;
			}
			else
			{
				coinsCurrent -= (coinsCurrent - coinsTarget) / 10;
			}
			base.Text = coinsCurrent.ToString();
		}
		bool value = base.IsShown && ELSingleton<IapManager>.Instance.ShowRewardedVideo(aIsExecute: false);
		base.gameObject.GetComponent<Animator>().SetBool("IsFreeCoins", value);
	}

	public new void Reset()
	{
		base.Reset();
		coinsCurrent = ELSingleton<CoinsManager>.Instance.Coins;
		coinsTarget = coinsCurrent;
		base.Text = coinsCurrent.ToString();
		shineParticle.SetActive(value: false);
	}

	private new void ShowEnded()
	{
		base.ShowEnded();
		shineParticle.SetActive(value: true);
	}

	public void AddCoins(int aCoins = 1)
	{
		coinsTarget += aCoins;
		if (coinsTarget < 0)
		{
			coinsTarget = 0;
		}
		else if (coinsTarget > ELSingleton<CoinsManager>.Instance.Coins)
		{
			coinsTarget = ELSingleton<CoinsManager>.Instance.Coins;
		}
	}

	public void UpdateCoins(int aSubCoins)
	{
		coinsCurrent = ELSingleton<CoinsManager>.Instance.Coins + aSubCoins;
		coinsTarget = coinsCurrent;
		base.Text = coinsCurrent.ToString();
	}
}
