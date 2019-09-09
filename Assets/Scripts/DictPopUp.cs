using System;
using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class DictPopUp : CommonPopUp<DictPopUp>
{
	private const float SCROLL_EXTRA_SIZE_IPHONE_5 = 400f;

	private const float SCROLL_EXTRA_SIZE_IPHONE_X = 450f;

	public RectTransform background;

	public ScrollRect scrollRect;

	public GameObject content;

	public Transform title;

	public CommonButton exitButton;

	public CommonButton searchByGoogleButton;

	public Text text;

	private string lastWord;

	public new void Start()
	{
		base.Start();
		RectTransform component = scrollRect.transform.GetComponent<RectTransform>();
		float num = (ELSingleton<ELCanvas>.Instance.CanvasRatio == ELCanvas.Ratio.iPhone5) ? 400f : ((ELSingleton<ELCanvas>.Instance.CanvasRatio == ELCanvas.Ratio.iPhoneX) ? 450f : 0f);
		background.sizeDelta = new Vector2(background.sizeDelta.x, background.sizeDelta.y + num);
		component.sizeDelta = new Vector2(component.sizeDelta.x, component.sizeDelta.y + num);
		title.localPosition = new Vector3(title.localPosition.x, title.localPosition.y + num / 2f, 0f);
		exitButton.transform.localPosition = new Vector3(exitButton.transform.localPosition.x, exitButton.transform.localPosition.y + num / 2f, 0f);
	}

	private new void Update()
	{
		base.Update();
		if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
		{
			ExitButton();
		}
	}

	public bool isChar(char c)
	{
		if ((c < 'a' || c > 'z') && (c < 'A' || c > 'Z') && (c < '0' || c > '9') && c != '[')
		{
			return c == '(';
		}
		return true;
	}

	public void ShowPopUp(string word, float aDelay = 0f)
	{
		ShowPopUp(aDelay);
		exitButton.Enable();
		this.text.text = "Searching...\n\n\n";
		lastWord = word;
		int num = 1;
		TextAsset textAsset = Resources.Load($"Dict/dict_{word.Substring(0, 1).ToUpper(CultureInfo.InvariantCulture)}") as TextAsset;
		if ((bool)textAsset)
		{
			string text = textAsset.text;
			string text2 = "<size=100>" + word + "</size>\n\n";
			int startIndex = 0;
			string value = "";
			while (true)
			{
				int num2 = text.IndexOf("|" + word + " ", startIndex, StringComparison.InvariantCultureIgnoreCase);
				if (num2 <= -1)
				{
					break;
				}
				num2 += word.Length;
				string text3 = textAsset.text.Substring(num2 + 2, 1);
				int num3 = text.IndexOf("|", num2 + 1, StringComparison.InvariantCultureIgnoreCase);
				if (num3 < 0)
				{
					num3 = text.Length;
				}
				startIndex = num3;
				string text4 = textAsset.text.Substring(num2 + 4, num3 - num2 - 5);
				if (!text3.Equals(value))
				{
					text2 = text2 + "<size=80>" + GetTypeName(text3) + "</size>\n";
					value = text3;
				}
				text2 = text2 + num + ". " + text4 + "\n\n";
				num++;
			}
			if (text2.Length > 0)
			{
				this.text.text = text2 + "\n\n\n";
			}
			else
			{
				this.text.text = "Ehh... :)\nWe can't find word definition.\n\n\n";
			}
			Resources.UnloadUnusedAssets();
		}
		StartCoroutine(SetScrollValue());
	}

	private string GetTypeName(string type)
	{
		if (!(type == "n"))
		{
			if (!(type == "v"))
			{
				if (!(type == "a"))
				{
					if (!(type == "s"))
					{
						if (type == "r")
						{
							return "Adverb";
						}
						return type;
					}
					return "Adjective Satellite";
				}
				return "Adjective";
			}
			return "Verb";
		}
		return "Noun";
	}

	private IEnumerator SetScrollValue()
	{
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		scrollRect.verticalNormalizedPosition = 1f;
		yield return null;
	}

	public void SearchByGoogle()
	{
		Application.OpenURL("https://www.google.com/search?hl=en&q=define+" + lastWord);
	}

	public void ExitButton()
	{
		exitButton.Disable();
		HidePopUp();
	}
}
