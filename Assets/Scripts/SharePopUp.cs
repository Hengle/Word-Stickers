using UnityEngine;
using UnityEngine.UI;

public class SharePopUp : CommonPopUp<SharePopUp>
{
	public CommonButton exitButton;

	public GameObject coinsIcon;

	public Text coinsText;

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
		bool flag = ELSingleton<ShareManager>.Instance.HasCoinsReward();
		coinsIcon.SetActive(flag);
		coinsText.gameObject.SetActive(flag);
		if (flag)
		{
			coinsText.text = "+" + ELSingleton<XmlSettings>.Instance.shareConfig.coins;
		}
		base.ShowPopUp(aDelay);
		exitButton.Enable();
	}

	public void ExitButton()
	{
		exitButton.Disable();
		HidePopUp();
	}

	public void ShareButton()
	{
		ShareAction();
		HidePopUp();
	}

	public void ShareAction()
	{
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("android.content.Intent");
		AndroidJavaObject androidJavaObject = new AndroidJavaObject("android.content.Intent");
		androidJavaObject.Call<AndroidJavaObject>("setAction", new object[1]
		{
			androidJavaClass.GetStatic<string>("ACTION_SEND")
		});
		androidJavaObject.Call<AndroidJavaObject>("setType", new object[1]
		{
			"text/plain"
		});
		androidJavaObject.Call<AndroidJavaObject>("putExtra", new object[2]
		{
			androidJavaClass.GetStatic<string>("EXTRA_SUBJECT"),
			ELSingleton<LanguageSettings>.Instance.GetString(0)
		});
		androidJavaObject.Call<AndroidJavaObject>("putExtra", new object[2]
		{
			androidJavaClass.GetStatic<string>("EXTRA_TEXT"),
			ELSingleton<LanguageSettings>.Instance.GetString(1)
		});
		AndroidJavaObject @static = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
		AndroidJavaObject androidJavaObject2 = androidJavaClass.CallStatic<AndroidJavaObject>("createChooser", new object[2]
		{
			androidJavaObject,
			"Share Via"
		});
		@static.Call("startActivity", androidJavaObject2);
		if (ELSingleton<ShareManager>.Instance.HasCoinsReward())
		{
			ELSingleton<CoinsManager>.Instance.AddCoins(ELSingleton<XmlSettings>.Instance.shareConfig.coins);
			XmlSettings.IapConfig iapConfig = new XmlSettings.IapConfig();
			iapConfig.coins = ELSingleton<XmlSettings>.Instance.shareConfig.coins;
			ELSingleton<IapManager>.Instance.CollectItems(iapConfig);
		}
		if (ELSingleton<SharePopUp>.Instance.isActiveAndEnabled)
		{
			ELSingleton<SharePopUp>.Instance.HidePopUp();
		}
		ELSingleton<ShareManager>.Instance.Consume();
		ELSingleton<MenuWindow>.Instance.ShareUpdate();
	}
}
