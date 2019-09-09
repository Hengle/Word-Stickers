using UnityEngine;

public class GDPRPopUp : CommonPopUp<GDPRPopUp>
{
	private new void Update()
	{
		base.Update();
		if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
		{
			OKButton();
		}
	}

	public new void ShowPopUp(float aDelay = 0f)
	{
		base.ShowPopUp(aDelay);
	}

	public void OKButton()
	{
		HidePopUp();
		ELSingleton<GDPRManager>.Instance.GDPRConsume();
		ELSingleton<ApplicationManager>.Instance.ShowGameAfterLoading();
	}

	public void PrivacyButton()
	{
		Application.OpenURL(ELSingleton<XmlSettings>.Instance.urlConfig.privacy);
	}
}
