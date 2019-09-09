using UnityEngine;
using UnityEngine.UI;

public class ExtraWordsPopUp : CommonPopUp<ExtraWordsPopUp>
{
	public Text coinsText;

	public Image progressBottom;

	public Image progressTop;

	public Text progressText;

	public CommonButton exitButton;

	public CommonButton collectButton;

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
		coinsText.text = $"{ELSingleton<ExtraWordsManager>.Instance.Coins.ToString()} COINS";
		if (ELSingleton<ExtraWordsManager>.Instance.WordsMax > 0)
		{
			progressTop.rectTransform.sizeDelta = new Vector3(progressBottom.rectTransform.sizeDelta.x * (float)ELSingleton<ExtraWordsManager>.Instance.WordsCurrent / (float)ELSingleton<ExtraWordsManager>.Instance.WordsMax, progressTop.rectTransform.sizeDelta.y);
		}
		progressText.text = $"{ELSingleton<ExtraWordsManager>.Instance.WordsCurrent}/{ELSingleton<ExtraWordsManager>.Instance.WordsMax}";
		exitButton.Enable();
		collectButton.gameObject.SetActive(ELSingleton<ExtraWordsManager>.Instance.IsFull);
	}

	public void ExitButton()
	{
		exitButton.Disable();
		HidePopUp();
	}

	public void CollectButton()
	{
		ELSingleton<ExtraWordsManager>.Instance.Collect();
		ExitButton();
	}
}
