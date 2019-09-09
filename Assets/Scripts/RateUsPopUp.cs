using UnityEngine;
using UnityEngine.UI;

public class RateUsPopUp : CommonPopUp<RateUsPopUp>
{
	public CommonButton exitButton;

	public Text coinsText;

	public GameObject coins;

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
		coinsText.text = "+" + ELSingleton<XmlSettings>.Instance.rateItConfig.coins;
		coins.SetActive(!ELSingleton<RateUsManager>.Instance.consumed);
		coinsText.gameObject.SetActive(!ELSingleton<RateUsManager>.Instance.consumed);
		base.ShowPopUp(aDelay);
		exitButton.Enable();
	}

	public void ExitButton()
	{
		exitButton.Disable();
		HidePopUp();
	}

	public void RateButton()
	{
		RateAction();
	}

	public void RateAction()
	{
		if (ELSingleton<ApplicationSettings>.Instance.shopVersion == ShopVersion.GooglePlay)
		{
			Application.OpenURL("market://details?id=" + ELSingleton<XmlSettings>.Instance.rateItConfig.androidAppId);
		}
		else if (ELSingleton<ApplicationSettings>.Instance.shopVersion == ShopVersion.IOS)
		{
			Application.OpenURL("itms-apps://itunes.apple.com/app/id" + ELSingleton<XmlSettings>.Instance.rateItConfig.iosAppId + "?action=write-review");
		}
		if (!ELSingleton<RateUsManager>.Instance.consumed)
		{
			ELSingleton<CoinsManager>.Instance.AddCoins(ELSingleton<XmlSettings>.Instance.rateItConfig.coins);
			XmlSettings.IapConfig iapConfig = new XmlSettings.IapConfig();
			iapConfig.coins = ELSingleton<XmlSettings>.Instance.rateItConfig.coins;
			ELSingleton<IapManager>.Instance.CollectItems(iapConfig);
		}
		if (ELSingleton<RateUsPopUp>.Instance.isActiveAndEnabled)
		{
			ELSingleton<RateUsPopUp>.Instance.HidePopUp();
		}
		ELSingleton<RateUsManager>.Instance.Consume();
		ELSingleton<MenuWindow>.Instance.RateUsUpdate();
	}
}
