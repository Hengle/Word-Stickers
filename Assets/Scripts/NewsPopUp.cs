using UnityEngine;

public class NewsPopUp : CommonPopUp<NewsPopUp>
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
}
