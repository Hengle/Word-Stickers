using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelSuccessPopUp : CommonPopUp<LevelSuccessPopUp>, IPointerDownHandler, IEventSystemHandler
{
	private const float BUTTON_OFFSET_X = 160f;

	public RectTransform pod;

	public GameObject backgroundNormal;

	public GameObject backgroundSpecial;

	public GameObject ribbonNormal;

	public GameObject ribbonSpecial;

	public GameObject textNormal;

	public GameObject textBonusRound;

	public GameObject textDailyPuzzle;

	public GameObject perfect;

	public RectTransform levelsLeft;

	public Text levelsLeftText;

	public GameObject nextButton;

	public CommonButton nextLevelButton;

	public CommonButton bonusRoundButton;

	public CommonButton continueButton;

	public CommonButton replayButton;

	public RectTransform sticker;

	public GameObject icon;

	public GameObject collected;

	public GameObject extralWords;

	public TextMeshProUGUI wordScoreAmount;

	public TextMeshProUGUI levelWordsAmount;

	public TextMeshProUGUI extraWordsAmount;

	public AudioClip soundIconShow;

	public AudioClip soundLevelSuccess;

	private bool isWordScoreUpdate;

	private float wordScoreDelay;

	private int wordScoreCurrent;

	private int wordScoreTarget;

	private int levelWords;

	private int extraWords;

	private int popIndex;

	private bool isSkip;

	public void OnPointerDown(PointerEventData aEventData)
	{
		if (isSkip)
		{
			base.gameObject.GetComponent<Animator>().speed = 8f;
			isSkip = false;
		}
	}

	private new void Start()
	{
		base.Start();
		if (ELSingleton<ELCanvas>.Instance.CanvasRatio == ELCanvas.Ratio.iPad)
		{
			pod.localScale = new Vector2(0.9f, 0.9f);
			sticker.localScale = new Vector2(0.9f, 0.9f);
			perfect.transform.localScale = new Vector2(0.9f, 0.9f);
			levelsLeft.transform.localPosition = new Vector3(levelsLeft.transform.position.x, -720f, 0f);
		}
		else if (ELSingleton<ELCanvas>.Instance.CanvasRatio == ELCanvas.Ratio.iPhone)
		{
			levelsLeft.transform.localPosition = new Vector3(levelsLeft.transform.position.x, -720f, 0f);
		}
	}

	private new void Update()
	{
		base.Update();
		if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
		{
			if (nextLevelButton.isActiveAndEnabled && nextLevelButton.IsEnabled)
			{
				NextLevelButton();
			}
			if (bonusRoundButton.isActiveAndEnabled && bonusRoundButton.IsEnabled)
			{
				BonusRoundButton();
			}
			if (continueButton.isActiveAndEnabled && continueButton.IsEnabled)
			{
				ContinueButton();
			}
		}
		if (isWordScoreUpdate)
		{
			if (wordScoreDelay < 0.05f)
			{
				wordScoreDelay += Time.deltaTime;
			}
			else if (wordScoreCurrent < wordScoreTarget)
			{
				wordScoreDelay = 0f;
				wordScoreCurrent++;
			}
			else
			{
				isWordScoreUpdate = false;
				wordScoreDelay = 0f;
				wordScoreCurrent = wordScoreTarget;
			}
			wordScoreAmount.text = wordScoreCurrent.ToString();
		}
	}

	public void ShowPopUp(LevelType aLevelType, LevelStats aLevelStats, bool aIsLastLevelInPack, bool aIsLevelCompleted, LevelStats aStats, float aDelay = 0f)
	{
		ShowPopUp(aDelay);
		icon.GetComponent<Image>().sprite = ELSingleton<GameWindow>.Instance.board.icon.sprite;
		icon.GetComponent<Image>().SetNativeSize();
		nextLevelButton.Enable();
		bonusRoundButton.Enable();
		continueButton.Enable();
		replayButton.Enable();
		switch (aLevelType)
		{
		case LevelType.Normal:
		{
			backgroundNormal.SetActive(value: true);
			backgroundSpecial.SetActive(value: false);
			ribbonNormal.SetActive(value: true);
			ribbonSpecial.SetActive(value: false);
			textNormal.SetActive(value: true);
			textBonusRound.SetActive(value: false);
			textDailyPuzzle.SetActive(value: false);
			collected.SetActive(value: true);
			Pack currentPack = ELSingleton<LevelsSettings>.Instance.levelSet.GetCurrentPack();
			int levelsToCompleteCount = currentPack.GetLevelsToCompleteCount();
			if (levelsToCompleteCount > 1)
			{
				levelsLeftText.gameObject.SetActive(value: true);
				levelsLeftText.text = $"{levelsToCompleteCount} levels left in {currentPack.name}";
			}
			else if (levelsToCompleteCount > 0)
			{
				levelsLeftText.gameObject.SetActive(value: true);
				levelsLeftText.text = $"{levelsToCompleteCount} level left in {currentPack.name}";
			}
			else
			{
				levelsLeftText.gameObject.SetActive(value: false);
			}
			if (!aIsLevelCompleted && aStats.isPerfect)
			{
				perfect.SetActive(value: true);
			}
			else
			{
				perfect.SetActive(value: false);
			}
			nextLevelButton.gameObject.SetActive(!aIsLastLevelInPack | aIsLevelCompleted);
			bonusRoundButton.gameObject.SetActive(aIsLastLevelInPack && !aIsLevelCompleted);
			continueButton.gameObject.SetActive(value: false);
			replayButton.gameObject.SetActive(value: false);
			nextButton.transform.localPosition = new Vector3(0f, nextButton.transform.localPosition.y, 0f);
			replayButton.transform.localPosition = new Vector3(160f, replayButton.transform.localPosition.y, 0f);
			extralWords.SetActive(value: true);
			break;
		}
		case LevelType.BonusRound:
		{
			backgroundNormal.SetActive(value: false);
			backgroundSpecial.SetActive(value: true);
			ribbonNormal.SetActive(value: false);
			ribbonSpecial.SetActive(value: true);
			textNormal.SetActive(value: false);
			textBonusRound.SetActive(value: true);
			textDailyPuzzle.SetActive(value: false);
			collected.SetActive(value: false);
			levelsLeftText.gameObject.SetActive(value: false);
			perfect.SetActive(value: false);
			bool flag2 = ELSingleton<AdsManager>.Instance.IsRewardedVideoAd && ELSingleton<GameWindow>.Instance.IsReplayAvailable;
			nextLevelButton.gameObject.SetActive(value: false);
			bonusRoundButton.gameObject.SetActive(value: false);
			continueButton.gameObject.SetActive(value: true);
			replayButton.gameObject.SetActive(flag2);
			nextButton.transform.localPosition = new Vector3(flag2 ? (-160f) : 0f, nextButton.transform.localPosition.y, 0f);
			replayButton.transform.localPosition = new Vector3(160f, replayButton.transform.localPosition.y, 0f);
			extralWords.SetActive(value: false);
			break;
		}
		case LevelType.DailyPuzzle:
		{
			backgroundNormal.SetActive(value: false);
			backgroundSpecial.SetActive(value: true);
			ribbonNormal.SetActive(value: false);
			ribbonSpecial.SetActive(value: true);
			textNormal.SetActive(value: false);
			textBonusRound.SetActive(value: false);
			textDailyPuzzle.SetActive(value: true);
			collected.SetActive(value: false);
			levelsLeftText.gameObject.SetActive(value: false);
			perfect.SetActive(value: false);
			bool flag = ELSingleton<AdsManager>.Instance.IsRewardedVideoAd && ELSingleton<GameWindow>.Instance.IsReplayAvailable;
			nextLevelButton.gameObject.SetActive(value: false);
			bonusRoundButton.gameObject.SetActive(value: false);
			continueButton.gameObject.SetActive(value: true);
			replayButton.gameObject.SetActive(flag);
			nextButton.transform.localPosition = new Vector3(flag ? (-160f) : 0f, nextButton.transform.localPosition.y, 0f);
			replayButton.transform.localPosition = new Vector3(160f, replayButton.transform.localPosition.y, 0f);
			extralWords.SetActive(value: true);
			break;
		}
		}
		isWordScoreUpdate = false;
		wordScoreDelay = 0f;
		wordScoreCurrent = ELSingleton<PointsManager>.Instance.Points - aLevelStats.pointsNormal - aLevelStats.pointsExtra;
		wordScoreTarget = wordScoreCurrent;
		levelWords = aLevelStats.pointsNormal;
		extraWords = aLevelStats.pointsExtra;
		popIndex = 0;
		isSkip = false;
		wordScoreAmount.text = wordScoreCurrent.ToString();
		levelWordsAmount.text = aLevelStats.pointsNormal.ToString();
		extraWordsAmount.text = aLevelStats.pointsExtra.ToString();
		base.gameObject.GetComponent<Animator>().speed = 1f;
	}

	private new void ShowPopUpBegan()
	{
		base.ShowPopUpBegan();
		ELSingleton<GameWindow>.Instance.board.icon.gameObject.SetActive(value: false);
		isSkip = true;
	}

	private new void ShowPopUpEnded()
	{
		base.ShowPopUpEnded();
		ELSingleton<GameWindow>.Instance.LevelSuccessPopUpShowHasEnded();
		isSkip = false;
		base.gameObject.GetComponent<Animator>().speed = 1f;
	}

	private void SoundIconShow()
	{
		ELSingleton<AudioManager>.Instance.PlaySfx(soundIconShow);
	}

	private void SoundLevelSuccess()
	{
		ELSingleton<AudioManager>.Instance.PlaySfx(soundLevelSuccess);
	}

	public void NextLevelButton()
	{
		nextLevelButton.Disable();
		bonusRoundButton.Disable();
		continueButton.Disable();
		replayButton.Disable();
		ELSingleton<GameWindow>.Instance.LevelSuccessPopUpNextLevelButton();
	}

	public void BonusRoundButton()
	{
		nextLevelButton.Disable();
		bonusRoundButton.Disable();
		continueButton.Disable();
		replayButton.Disable();
		ELSingleton<GameWindow>.Instance.LevelSuccessPopUpNextLevelButton();
	}

	public void ContinueButton()
	{
		nextLevelButton.Disable();
		bonusRoundButton.Disable();
		continueButton.Disable();
		replayButton.Disable();
		ELSingleton<GameWindow>.Instance.LevelSuccessPopUpContinueButton();
	}

	public void ReplayButton()
	{
		nextLevelButton.Disable();
		bonusRoundButton.Disable();
		continueButton.Disable();
		replayButton.Disable();
		ELSingleton<GameWindow>.Instance.LevelSuccessPopUpReplayButton();
	}

	public void PopWordScore()
	{
		if (popIndex == 0)
		{
			isWordScoreUpdate = true;
			wordScoreDelay = 0f;
			wordScoreTarget += levelWords;
		}
		if (popIndex == 1)
		{
			isWordScoreUpdate = true;
			wordScoreDelay = 0f;
			wordScoreTarget += extraWords;
		}
		popIndex++;
	}
}
