using UnityEngine;

public class FacebookPopUp : CommonPopUp<FacebookPopUp>
{
	public CommonButton exitButton;

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
		base.ShowPopUp(aDelay);
		exitButton.Enable();
	}

	public void ExitButton()
	{
		exitButton.Disable();
		HidePopUp();
	}

	public void ConnectButton()
	{
		ELSingleton<FacebookManager>.Instance.connectedAction(ELSingleton<MenuWindow>.Instance, "FBConnectCallback", firstConnect: true);
	}
}
