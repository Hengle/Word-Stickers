using UnityEngine;
using UnityEngine.UI;

public class SettingsPopUp : CommonPopUp<SettingsPopUp>
{
	public CommonButton exitButton;

	public CommonButton restoreButton;

	public CommonToggle musicToggle;

	public CommonToggle sfxToggle;

	public Text fbButtonText;

	public Text versionText;

	public CommonButton[] buttonsToAlign;

	private new void Update()
	{
		base.Update();
		if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
		{
			ExitButton();
		}
		int num = 0;
		CommonButton[] array = buttonsToAlign;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].isActiveAndEnabled)
			{
				num++;
			}
		}
		float num2 = (buttonsToAlign[buttonsToAlign.Length - 1].GetComponent<RectTransform>().localPosition.y - buttonsToAlign[0].GetComponent<RectTransform>().localPosition.y) / (float)(num - 1);
		float num3 = buttonsToAlign[0].GetComponent<RectTransform>().localPosition.y + num2;
		for (int j = 1; j < buttonsToAlign.Length - 1; j++)
		{
			RectTransform component = buttonsToAlign[j].GetComponent<RectTransform>();
			component.localPosition = new Vector2(component.localPosition.x, num3);
			if (buttonsToAlign[j].isActiveAndEnabled)
			{
				num3 += num2;
			}
		}
	}

	public new void ShowPopUp(float aDelay = 0f)
	{
		restoreButton.gameObject.SetActive(value: false);
		fbButtonText.text = (ELSingleton<FacebookManager>.Instance.connected ? "Disconnect" : "Connect");
		base.ShowPopUp(aDelay);
		exitButton.Enable();
		musicToggle.SetToggle(!ELSingleton<AudioManager>.Instance.musicMute, animate: false);
		sfxToggle.SetToggle(!ELSingleton<AudioManager>.Instance.sfxMute, animate: false);
		versionText.text = $"v{Application.version}";
	}

	public void ExitButton()
	{
		exitButton.Disable();
		HidePopUp();
	}

	public void MusicButton()
	{
		ELSingleton<AudioManager>.Instance.musicMute = !ELSingleton<AudioManager>.Instance.musicMute;
		if (!ELSingleton<AudioManager>.Instance.musicMute)
		{
			ELSingleton<MusicManager>.Instance.PlayMenu();
		}
		ELSingleton<ApplicationSettings>.Instance.Save();
	}

	public void SfxButton()
	{
		ELSingleton<AudioManager>.Instance.sfxMute = !ELSingleton<AudioManager>.Instance.sfxMute;
		ELSingleton<ApplicationSettings>.Instance.Save();
		ELSingleton<AudioManager>.Instance.PlaySfx(sfxToggle.clickSound);
	}

	public void ShareButton()
	{
		ELSingleton<SharePopUp>.Instance.ShareAction();
		HidePopUp();
	}

	public void RateUsButton()
	{
		ELSingleton<RateUsPopUp>.Instance.RateAction();
		HidePopUp();
	}

	public void ConnectButton()
	{
		if (ELSingleton<FacebookManager>.Instance.connected)
		{
			HidePopUp();
			ELSingleton<FacebookManager>.Instance.FBDisconnect();
			ELSingleton<MenuWindow>.Instance.FBDisconnect();
		}
		else
		{
			HidePopUp();
			ELSingleton<FacebookPopUp>.Instance.ShowPopUp(0.5f);
		}
	}

	public void ResttorePurchasesButton()
	{
		HidePopUp();
		ELSingleton<IapManager>.Instance.RestorePurchases();
	}

	public void AboutButton()
	{
		Application.OpenURL(ELSingleton<XmlSettings>.Instance.urlConfig.about);
	}

	public void PrivacyButton()
	{
		Application.OpenURL(ELSingleton<XmlSettings>.Instance.urlConfig.privacy);
	}

	public void HowToPlayButton()
	{
		HidePopUp();
		ELSingleton<HowToPlayPopUp>.Instance.ShowPopUp(0.2f);
	}
}
