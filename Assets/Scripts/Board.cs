using System;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
	public enum WordCheckResult
	{
		Valid,
		Invalid,
		ExtraWord,
		Repeat,
		None
	}

	private const int WORD_LENGTH_MAX = 8;

	private const float PLAY_AREA_WIDTH = 900f;

	private const float PLAY_AREA_HEIGHT = 900f;

	private const int WORD_LETTER_OFFSET = 120;

	private const int FILL_A_WIDTH = 160;

	private const int FILL_A_HEIGHT = 180;

	private const int FILL_B_WIDTH = 180;

	private const int FILL_B_HEIGHT = 202;

	public RawImage fill;

	public Image icon;

	public Texture2D fillTextureA;

	public Texture2D fillTextureB;

	public GameObject wordPod;

	public WordText wordText;

	public BoardWord boardWordPrefab;

	private Level level;

	private int levelWidth;

	private int levelHeight;

	private float levelScale;

	private bool isWordPodScaleUpdate;

	private float wordPodScaleCurrent;

	private float wordPodScaleTarget;

	private bool isEnabled;

	private bool isReset;

	private int selectedX;

	private int selectedY;

	private BoardLetter selectedLetter;

	private int selectedIndex;

	private float shineDelay;

	public bool IsWordValid
	{
		get;
		set;
	}

	public WordCheckResult Result
	{
		get;
		private set;
	}

	private void Update()
	{
		if (isEnabled && !IsWordValid && (!ELSingleton<TutorialWindow>.Instance.isActiveAndEnabled || !ELSingleton<TutorialWindow>.Instance.isPointerDown))
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			Vector3 position = Vector3.zero;
			if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
			{
				if (UnityEngine.Input.touchCount > 0)
				{
					flag = (UnityEngine.Input.GetTouch(0).phase == TouchPhase.Began);
					flag2 = (UnityEngine.Input.GetTouch(0).phase == TouchPhase.Moved);
					flag3 = (UnityEngine.Input.GetTouch(0).phase == TouchPhase.Ended);
					position = UnityEngine.Input.GetTouch(0).position;
				}
			}
			else
			{
				flag = Input.GetMouseButtonDown(0);
				flag2 = (Input.GetMouseButton(0) && (UnityEngine.Input.GetAxis("Mouse X") != 0f || UnityEngine.Input.GetAxis("Mouse Y") != 0f));
				flag3 = Input.GetMouseButtonUp(0);
				position = UnityEngine.Input.mousePosition;
			}
			if (flag | flag2)
			{
				RaycastHit2D[] array = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(position), Camera.main.transform.forward);
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i].collider != null)
					{
						if (isReset)
						{
							wordText.ResetText();
							wordText.CalculateFill();
							wordText.Idle();
							isReset = false;
							selectedX = int.MinValue;
							selectedY = int.MinValue;
							selectedLetter = null;
							selectedIndex = 0;
						}
						BoardLetter component = array[i].collider.GetComponent<BoardLetter>();
						if (component != null && component.IsAvailable && ((selectedX == int.MinValue && selectedY == int.MinValue) || (selectedX == component.X && (selectedY == component.Y + 1 || selectedY == component.Y - 1)) || (selectedY == component.Y && (selectedX == component.X + 1 || selectedX == component.X - 1))) && !component.IsSelected && !component.IsMarked && wordText.Text.Length < 8)
						{
							wordText.Text += component.Letter;
							wordText.CalculateFill();
							component.Select(selectedIndex, (selectedX == component.X - 1) ? BoardLetter.SelectorDirection.Left : ((selectedX == component.X + 1) ? BoardLetter.SelectorDirection.Right : ((selectedY == component.Y - 1) ? BoardLetter.SelectorDirection.Up : ((selectedY == component.Y + 1) ? BoardLetter.SelectorDirection.Down : BoardLetter.SelectorDirection.None))));
							selectedX = component.X;
							selectedY = component.Y;
							selectedLetter = component;
							selectedIndex++;
						}
						else if (component != null && ((selectedX == component.X && (selectedY == component.Y + 1 || selectedY == component.Y - 1)) || (selectedY == component.Y && (selectedX == component.X + 1 || selectedX == component.X - 1))) && component.IsSelected && !component.IsMarked && component.SelectedIndex == selectedIndex - 2 && selectedLetter != null)
						{
							wordText.Text = wordText.Text.Substring(0, wordText.Text.Length - 1);
							wordText.CalculateFill();
							selectedLetter.UnSelect(aIsSound: true);
							selectedX = component.X;
							selectedY = component.Y;
							selectedLetter = component;
							selectedIndex--;
						}
					}
				}
				if (array.Length == 0)
				{
					flag3 = true;
				}
				ELSingleton<TutorialWindow>.Instance.Hide();
			}
			if (!isReset && flag3)
			{
				IsWordValid = true;
				isReset = true;
			}
		}
		if (shineDelay <= 0f)
		{
			shineDelay = UnityEngine.Random.Range(5, 10);
			BoardLetter[] componentsInChildren = base.gameObject.GetComponentsInChildren<BoardLetter>();
			int num = UnityEngine.Random.Range(1, 6);
			for (int j = 0; j < num; j++)
			{
				int num2 = UnityEngine.Random.Range(0, componentsInChildren.Length);
				componentsInChildren[num2].Shine((float)j * 0.2f);
			}
		}
		else
		{
			shineDelay -= Time.deltaTime;
		}
		if (!isWordPodScaleUpdate)
		{
			return;
		}
		if (wordPodScaleCurrent <= wordPodScaleTarget)
		{
			wordPodScaleCurrent += 1f * Time.deltaTime;
			if (wordPodScaleCurrent > wordPodScaleTarget)
			{
				wordPodScaleCurrent = wordPodScaleTarget;
				isWordPodScaleUpdate = false;
			}
		}
		else if (wordPodScaleCurrent > wordPodScaleTarget)
		{
			wordPodScaleCurrent -= 1f * Time.deltaTime;
			if (wordPodScaleCurrent < wordPodScaleTarget)
			{
				wordPodScaleCurrent = wordPodScaleTarget;
				isWordPodScaleUpdate = false;
			}
		}
		wordPod.transform.localScale = new Vector3(wordPodScaleCurrent, wordPodScaleCurrent, 1f);
		wordPod.transform.localPosition = new Vector3((float)(-(levelWidth - 60)) * wordPodScaleCurrent * 0.5f, (float)(levelHeight - 60) * wordPodScaleCurrent * 0.5f, 0f);
	}

	public void Reset()
	{
		BoardWord[] componentsInChildren = wordPod.gameObject.GetComponentsInChildren<BoardWord>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			UnityEngine.Object.Destroy(componentsInChildren[i].gameObject);
		}
		fill.gameObject.SetActive(value: false);
		icon.gameObject.SetActive(value: false);
		wordText.Reset();
		wordText.ResetText();
		wordText.CalculateFill();
		levelWidth = 0;
		levelHeight = 0;
		levelScale = 0f;
		isWordPodScaleUpdate = false;
		wordPodScaleCurrent = 0f;
		wordPodScaleTarget = 0f;
		isEnabled = false;
		IsWordValid = false;
		Result = WordCheckResult.None;
		isReset = true;
		selectedX = int.MinValue;
		selectedY = int.MinValue;
		selectedLetter = null;
		selectedIndex = 0;
		shineDelay = UnityEngine.Random.Range(5, 10);
		CancelInvoke("EnableInvoke");
		CancelInvoke("ShowFillInvoke");
		CancelInvoke("HideFillInvoke");
		CancelInvoke("ShowIconInvoke");
		CancelInvoke("HideIconInvoke");
		CancelInvoke("ShowWordsInvoke");
		CancelInvoke("HideWordsInvoke");
		CancelInvoke("TipInvoke");
	}

	public void Enable(float aDelay = 0f)
	{
		Invoke("EnableInvoke", aDelay);
	}

	public void EnableInvoke()
	{
		if (base.isActiveAndEnabled)
		{
			isEnabled = true;
		}
	}

	public void Disable()
	{
		CancelInvoke("EnableInvoke");
		isEnabled = false;
	}

	public void Set(Pack aPack, Level aLevel)
	{
		level = aLevel;
		CalculateLevel(aLevel);
		AddFill(aLevel);
		AddIcon(aPack, aLevel);
		AddWords(aLevel);
		Invoke("TipInvoke", 2.5f);
	}

	private void CalculateLevel(Level aLevel)
	{
		levelWidth = aLevel.width * 120 + 60;
		levelHeight = aLevel.height * 120 + 82;
		float num = 900f / (float)levelWidth;
		float num2 = 900f / (float)levelHeight;
		levelScale = ((num > num2) ? num2 : num);
		isWordPodScaleUpdate = false;
		wordPodScaleCurrent = levelScale;
		wordPodScaleTarget = levelScale;
	}

	private void AddFill(Level aLevel)
	{
		Sprite sprite = ELSingleton<IconsManager>.Instance.LoadIconBack(ELSingleton<LevelsSettings>.Instance.levelSet.GetCurrentLevelInfo(), aLevel);
		if (sprite != null)
		{
			fill.texture = sprite.texture;
			fill.rectTransform.sizeDelta = new Vector3(levelWidth, levelHeight, 0f);
			fill.rectTransform.localScale = new Vector3(levelScale, levelScale, 1f);
			return;
		}
		Texture2D texture2D = new Texture2D(levelWidth, levelHeight, TextureFormat.ARGB32, mipChain: false);
		texture2D.filterMode = FilterMode.Point;
		for (int i = 0; i < levelWidth; i++)
		{
			for (int j = 0; j < levelHeight; j++)
			{
				texture2D.SetPixel(i, j, Color.clear);
			}
		}
		foreach (LevelWord word in aLevel.words)
		{
			foreach (LevelLetter letter in word.letters)
			{
				SetPixels(texture2D, fillTextureB, letter.x * 120, (aLevel.height - letter.y - 1) * 120, 180, 202);
			}
		}
		foreach (LevelWord word2 in aLevel.words)
		{
			foreach (LevelLetter letter2 in word2.letters)
			{
				SetPixels(texture2D, fillTextureA, 10 + letter2.x * 120, 11 + (aLevel.height - letter2.y - 1) * 120, 160, 180);
			}
		}
		texture2D.Apply();
		fill.texture = texture2D;
		fill.rectTransform.sizeDelta = new Vector3(levelWidth, levelHeight, 0f);
		fill.rectTransform.localScale = new Vector3(levelScale, levelScale, 1f);
	}

	private void SetPixels(Texture2D aMainTexture, Color32 aColor, int aX, int aY, int aSubWidth, int aSubHeight)
	{
		int width = aMainTexture.width;
		Color32[] pixels = aMainTexture.GetPixels32();
		for (int i = 0; i < aSubWidth; i++)
		{
			for (int j = 0; j < aSubHeight; j++)
			{
				int num = aY * width + aX + j * width + i;
				pixels[num] = ((aColor.a == 0) ? pixels[num] : aColor);
			}
		}
		aMainTexture.SetPixels32(pixels);
	}

	private void SetPixels(Texture2D aMainTexture, Texture2D aSubTexture, int aX, int aY, int aSubWidth, int aSubHeight)
	{
		int width = aMainTexture.width;
		Color32[] pixels = aMainTexture.GetPixels32();
		Color32[] pixels2 = aSubTexture.GetPixels32();
		for (int i = 0; i < aSubWidth; i++)
		{
			for (int j = 0; j < aSubHeight; j++)
			{
				int num = aY * width + aX + j * width + i;
				int num2 = j * aSubWidth + i;
				byte a = pixels[num].a;
				int a2 = pixels2[num2].a;
				if (a == 0)
				{
					pixels[num] = pixels2[num2];
					continue;
				}
				if (a2 == 255)
				{
					pixels[num] = pixels2[num2];
					continue;
				}
				int r = pixels[num].r;
				int g = pixels[num].g;
				int b = pixels[num].b;
				int a3 = pixels2[num2].a;
				byte r2 = pixels2[num2].r;
				int g2 = pixels2[num2].g;
				int b2 = pixels2[num2].b;
				int num3 = 255;
				int num4 = (r2 * a3 + r * (255 - a3)) / 255;
				int num5 = (g2 * a3 + g * (255 - a3)) / 255;
				int num6 = (b2 * a3 + b * (255 - a3)) / 255;
				pixels[num].a = (byte)num3;
				pixels[num].r = (byte)num4;
				pixels[num].g = (byte)num5;
				pixels[num].b = (byte)num6;
			}
		}
		aMainTexture.SetPixels32(pixels);
	}

	private void AddIcon(Pack aPack, Level aLevel)
	{
		Sprite sprite = ELSingleton<IconsManager>.Instance.LoadIcon(ELSingleton<LevelsSettings>.Instance.levelSet.GetCurrentLevelInfo(), aLevel);
		if (sprite == null)
		{
			ELSingleton<IconsManager>.Instance.Load(aPack.name);
			sprite = Sprite.Create(ELSingleton<IconsManager>.Instance.Icon.texture, new Rect(aLevel.iconRect.x, (float)ELSingleton<IconsManager>.Instance.Icon.texture.height - aLevel.iconRect.y - aLevel.iconRect.height, aLevel.iconRect.width, aLevel.iconRect.height), new Vector2(0f, 0f));
		}
		icon.sprite = sprite;
		icon.rectTransform.sizeDelta = new Vector2(aLevel.width * 120, aLevel.height * 120);
		icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, 0.25f);
		icon.transform.localScale = new Vector3(levelScale, levelScale, 1f);
	}

	private void AddWords(Level aLevel)
	{
		foreach (LevelWord word in aLevel.words)
		{
			UnityEngine.Object.Instantiate(boardWordPrefab, wordPod.transform).Set(word, 120f, aLevel.isCompleted, aLevel.type);
		}
		wordPod.transform.localScale = new Vector3(levelScale, levelScale, 1f);
		wordPod.transform.localPosition = new Vector3((float)(-(levelWidth - 60)) * levelScale * 0.5f, (float)(levelHeight - 60) * levelScale * 0.5f, 0f);
	}

	public void SetSpecialLevelRound(int aSpecialLevelRound, bool aIsScaleForce)
	{
		float num = (level.width - aSpecialLevelRound * 2) * 120 + 60;
		float num2 = (level.height - aSpecialLevelRound * 2) * 120 + 82;
		float num3 = 900f / num;
		float num4 = 900f / num2;
		isWordPodScaleUpdate = true;
		wordPodScaleTarget = ((num3 > num4) ? num4 : num3);
		if (aIsScaleForce)
		{
			wordPodScaleCurrent = wordPodScaleTarget;
		}
		BoardLetter[] componentsInChildren = base.gameObject.GetComponentsInChildren<BoardLetter>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i].X < aSpecialLevelRound || componentsInChildren[i].X >= level.width - aSpecialLevelRound || componentsInChildren[i].Y < aSpecialLevelRound || componentsInChildren[i].Y >= level.height - aSpecialLevelRound)
			{
				componentsInChildren[i].IsAvailable = false;
			}
			else if (!aIsScaleForce)
			{
				componentsInChildren[i].IsAvailable = true;
				componentsInChildren[i].Show(UnityEngine.Random.Range(0f, 0.2f));
			}
		}
	}

	public bool HasLetter(int aX, int aY)
	{
		BoardLetter[] componentsInChildren = base.gameObject.GetComponentsInChildren<BoardLetter>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i].X == aX && componentsInChildren[i].Y == aY)
			{
				return true;
			}
		}
		return false;
	}

	public bool IsComplete()
	{
		BoardWord[] componentsInChildren = wordPod.gameObject.GetComponentsInChildren<BoardWord>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (!componentsInChildren[i].IsCompleted)
			{
				return false;
			}
		}
		return true;
	}

	public bool HasCoins()
	{
		BoardLetter[] componentsInChildren = base.gameObject.GetComponentsInChildren<BoardLetter>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i].HasCoin())
			{
				return true;
			}
		}
		return false;
	}

	public WordCheckResult CheckWordNormal(bool aIsLevelCompleted)
	{
		Result = WordCheckResult.None;
		BoardWord[] componentsInChildren = wordPod.gameObject.GetComponentsInChildren<BoardWord>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i].IsSelected() && string.Equals(wordText.Text, componentsInChildren[i].Word, StringComparison.OrdinalIgnoreCase))
			{
				componentsInChildren[i].IsCompleted = true;
				componentsInChildren[i].MarkLetters(aIsWithAnimation: true);
				componentsInChildren[i].RewardCoins();
				if (i == level.hintTipIndex)
				{
					ELSingleton<GameWindow>.Instance.tipText.Hide();
					level.hintTipIndex = -1;
				}
				ELSingleton<LevelsSettings>.Instance.Save();
				Result = WordCheckResult.Valid;
			}
			else
			{
				componentsInChildren[i].UnSelectLetters();
			}
		}
		if (Result == WordCheckResult.None && wordText.Text.Length > 0)
		{
			if (!aIsLevelCompleted && ELSingleton<DictionarySettings>.Instance.CheckWord(wordText.Text.ToLower()))
			{
				if (ELSingleton<LevelsSettings>.Instance.levelSet.IsExtraWord(wordText.Text.ToLower()))
				{
					Result = WordCheckResult.Repeat;
				}
				else
				{
					ELSingleton<LevelsSettings>.Instance.levelSet.AddExtraWord(wordText.Text.ToLower());
					Result = WordCheckResult.ExtraWord;
				}
			}
			else
			{
				Result = WordCheckResult.Invalid;
			}
		}
		if (Result == WordCheckResult.Valid)
		{
			wordText.Valid();
		}
		else if (Result == WordCheckResult.Invalid)
		{
			wordText.Invalid();
		}
		else if (Result == WordCheckResult.ExtraWord)
		{
			wordText.ExtraWord();
		}
		else if (Result == WordCheckResult.Repeat)
		{
			wordText.Repeat();
		}
		return Result;
	}

	public WordCheckResult CheckWordSpecial()
	{
		BoardLetter[] componentsInChildren = base.gameObject.GetComponentsInChildren<BoardLetter>();
		if (ELSingleton<DictionarySettings>.Instance.CheckWord(wordText.Text.ToLower()))
		{
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i].IsSelected)
				{
					componentsInChildren[i].Mark(aIsWithAnimation: true);
					componentsInChildren[i].RewardCoin();
					ELSingleton<PointsManager>.Instance.AddPoints(1);
				}
				else
				{
					componentsInChildren[i].UnSelect(aIsSound: false);
				}
			}
			wordText.Valid();
			return WordCheckResult.Valid;
		}
		for (int j = 0; j < componentsInChildren.Length; j++)
		{
			componentsInChildren[j].UnSelect(aIsSound: false);
		}
		wordText.Invalid();
		return WordCheckResult.Invalid;
	}

	public void ShowFill(float aDelay = 0f)
	{
		Invoke("ShowFillInvoke", aDelay);
	}

	private void ShowFillInvoke()
	{
		if (base.isActiveAndEnabled)
		{
			fill.gameObject.SetActive(value: true);
		}
	}

	public void HideFill(float aDelay = 0f)
	{
		Invoke("HideFillInvoke", aDelay);
	}

	private void HideFillInvoke()
	{
		if (base.isActiveAndEnabled)
		{
			fill.gameObject.SetActive(value: false);
		}
	}

	public void ShowIcon(float aDelay = 0f)
	{
		Invoke("ShowIconInvoke", aDelay);
	}

	private void ShowIconInvoke()
	{
		if (base.isActiveAndEnabled)
		{
			icon.gameObject.SetActive(value: true);
		}
	}

	public void HideIcon(float aDelay = 0f)
	{
		Invoke("HideIconInvoke", aDelay);
	}

	private void HideIconInvoke()
	{
		if (base.isActiveAndEnabled)
		{
			icon.gameObject.SetActive(value: false);
		}
	}

	public void ShowWords(float aDelay = 0f)
	{
		Invoke("ShowWordsInvoke", aDelay);
	}

	private void ShowWordsInvoke()
	{
		if (base.isActiveAndEnabled)
		{
			BoardWord[] componentsInChildren = base.gameObject.GetComponentsInChildren<BoardWord>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].ShowLetters();
			}
		}
	}

	public void HideWords(float aDelay = 0f)
	{
		Invoke("HideWordsInvoke", aDelay);
	}

	private void HideWordsInvoke()
	{
		if (base.isActiveAndEnabled)
		{
			BoardWord[] componentsInChildren = base.gameObject.GetComponentsInChildren<BoardWord>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].HideLetters();
			}
		}
	}

	public bool Hint(LevelHint aLevelHint)
	{
		if (aLevelHint <= LevelHint.Expose)
		{
			BoardWord[] componentsInChildren = base.gameObject.GetComponentsInChildren<BoardWord>();
			int num = UnityEngine.Random.Range(0, componentsInChildren.Length);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				int num2 = (num + i) % componentsInChildren.Length;
				if (componentsInChildren[num2].Hint(aLevelHint, aIsNoneOnly: true, aIsForce: false, aIsWithAnimation: true))
				{
					ELSingleton<LevelsSettings>.Instance.Save();
					return true;
				}
			}
			for (int j = 0; j < componentsInChildren.Length; j++)
			{
				int num3 = (num + j) % componentsInChildren.Length;
				if (componentsInChildren[num3].Hint(aLevelHint, aIsNoneOnly: false, aIsForce: false, aIsWithAnimation: true))
				{
					ELSingleton<LevelsSettings>.Instance.Save();
					return true;
				}
			}
		}
		else if (aLevelHint == LevelHint.Tip)
		{
			return Tip();
		}
		return false;
	}

	private bool Tip()
	{
		BoardWord[] componentsInChildren = base.gameObject.GetComponentsInChildren<BoardWord>();
		int num = UnityEngine.Random.Range(0, componentsInChildren.Length);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			int num2 = (num + i) % componentsInChildren.Length;
			if (!componentsInChildren[num2].IsCompleted && componentsInChildren[num2].HintType < LevelHint.Expose && level.hintTipIndex != num2)
			{
				TipExecute(componentsInChildren[num2].Word);
				level.hintTipIndex = num2;
				ELSingleton<LevelsSettings>.Instance.Save();
				return true;
			}
		}
		return false;
	}

	private void TipInvoke()
	{
		BoardWord[] componentsInChildren = base.gameObject.GetComponentsInChildren<BoardWord>();
		if (componentsInChildren != null && level.hintTipIndex >= 0 && level.hintTipIndex < componentsInChildren.Length - 1)
		{
			TipExecute(componentsInChildren[level.hintTipIndex].Word);
		}
	}

	private void TipExecute(string aWord)
	{
		if (aWord != null)
		{
			ELSingleton<GameWindow>.Instance.tipText.Text = aWord.ToUpper();
			ELSingleton<GameWindow>.Instance.tipText.CalculateFill();
			ELSingleton<GameWindow>.Instance.tipText.Show();
		}
	}
}
