using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessagePopUp : CommonPopUp<MessagePopUp>
{
	public CommonButton exitButton;

	public TextMeshProUGUI title;

	public Text msg;

	private new void Update()
	{
		base.Update();
		if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
		{
			ExitButton();
		}
	}

	public void ShowPopUp(string atitle, string amessage, float aDelay = 0f)
	{
		if (atitle != null)
		{
			title.text = atitle;
		}
		msg.text = amessage;
		ShowPopUp(aDelay);
		exitButton.Enable();
	}

	public void ExitButton()
	{
		exitButton.Disable();
		HidePopUp();
	}
}
