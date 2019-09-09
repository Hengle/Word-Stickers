using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameWindow : ELWindow<GameWindow>, IPointerDownHandler, IEventSystemHandler
{
	private enum State
	{
		LoadNormal,
		ReloadNormal,
		ShowLoadNormal,
		ShowReloadNormal,
		LoadBonusRound,
		ShowBonusRound,
		LoadDailyPuzzle,
		ShowDailyPuzzle,
		Play,
		ResetNormal,
		HideBonusRound,
		ResetBonusRound,
		HideDailyPuzzle,
		ResetDailyPuzzle,
		Idle
	}

	public GameBackground background;

	public Board board;

	public Ladybug ladybug;

	public CommonText levelText;

	public CommonText categoryText;

	public CommonButton backButton;

	public ShopButton shopButton;

	public ExtraWordsButton extraWordsButton;

	public PerfectMeter perfectMeter;

	public TimeMeter timeMeter;

	public HintBar hintBar;

	public HintButton goodStartButton;

	public HintButton startAndFinishButton;

	public HintButton exposeButton;

	public HintButton tipButton;

	public CommonText tipText;

	public SpecialWelcome specialWelcome;

	public CommonCoinPod coinPod;

	public AudioClip soundWordValid;

	public AudioClip soundWordInvalid;

	public AudioClip soundWordExtra;

	public AudioClip soundWordRepeat;

	public AudioClip soundHint;

	public AudioClip soundButtonGeneral;

	public AudioClip soundButtonX;

	public AudioClip soundSpecialLevelRound;

	private LevelType levelTypeLoad;

	private int packIndexLoad;

	private Level level;

	private Pack pack;

	private State state;

	private float delay;

	private bool isLevelCompleted;

	private bool isLastLevelInPack;

	private bool isPerfect;

	private int specialLevelRound;

	private bool isMusicPlaying;

	public bool IsHowToPlayPopUp
	{
		get;
		set;
	}

	public bool IsReplayAvailable
	{
		get;
		private set;
	}

	public void OnPointerDown(PointerEventData aEventData)
	{
		if (state == State.Play && (level.type == LevelType.Normal || level.type == LevelType.DailyPuzzle) && !categoryText.IsShowEnded)
		{
			Time.timeScale = 8f;
		}
	}

	private void Awake()
	{
		IsHowToPlayPopUp = true;
	}

	public new void Start()
	{
		base.Start();
		if (ELDevice.HasNotch())
		{
			levelText.transform.localPosition = new Vector3(levelText.transform.localPosition.x, levelText.transform.localPosition.y - ELDevice.notchOffsetY, 0f);
			categoryText.transform.localPosition = new Vector3(categoryText.transform.localPosition.x, categoryText.transform.localPosition.y - ELDevice.notchOffsetY, 0f);
			backButton.transform.localPosition = new Vector3(backButton.transform.localPosition.x, backButton.transform.localPosition.y - ELDevice.notchOffsetY, 0f);
			shopButton.transform.localPosition = new Vector3(shopButton.transform.localPosition.x, shopButton.transform.localPosition.y - ELDevice.notchOffsetY, 0f);
			extraWordsButton.transform.localPosition = new Vector3(extraWordsButton.transform.localPosition.x, extraWordsButton.transform.localPosition.y - ELDevice.notchOffsetY, 0f);
			goodStartButton.transform.localPosition = new Vector3(goodStartButton.transform.localPosition.x, goodStartButton.transform.localPosition.y + ELDevice.notchOffsetY, 0f);
			startAndFinishButton.transform.localPosition = new Vector3(startAndFinishButton.transform.localPosition.x, startAndFinishButton.transform.localPosition.y + ELDevice.notchOffsetY, 0f);
			exposeButton.transform.localPosition = new Vector3(exposeButton.transform.localPosition.x, exposeButton.transform.localPosition.y + ELDevice.notchOffsetY, 0f);
			tipButton.transform.localPosition = new Vector3(tipButton.transform.localPosition.x, tipButton.transform.localPosition.y + ELDevice.notchOffsetY, 0f);
			tipText.transform.localPosition = new Vector3(tipText.transform.localPosition.x, tipText.transform.localPosition.y + ELDevice.notchOffsetY, 0f);
			perfectMeter.transform.localPosition = new Vector3(perfectMeter.transform.localPosition.x, perfectMeter.transform.localPosition.y - ELDevice.notchOffsetY, 0f);
		}
		if (ELSingleton<ELCanvas>.Instance.CanvasRatio == ELCanvas.Ratio.iPhone)
		{
			board.transform.localScale = new Vector3(0.9f, 0.9f, 1f);
			timeMeter.transform.localScale = new Vector3(0.9f, 0.9f, 1f);
		}
		else if (ELSingleton<ELCanvas>.Instance.CanvasRatio == ELCanvas.Ratio.iPhone5)
		{
			extraWordsButton.transform.localScale = new Vector3(1.1f, 1.1f, 1f);
			extraWordsButton.transform.localPosition = new Vector3(extraWordsButton.transform.localPosition.x, extraWordsButton.transform.localPosition.y - 50f, 0f);
		}
		else if (ELSingleton<ELCanvas>.Instance.CanvasRatio == ELCanvas.Ratio.iPhoneX)
		{
			extraWordsButton.transform.localScale = new Vector3(1.1f, 1.1f, 1f);
			extraWordsButton.transform.localPosition = new Vector3(extraWordsButton.transform.localPosition.x, extraWordsButton.transform.localPosition.y - 50f, 0f);
		}
		else if (ELSingleton<ELCanvas>.Instance.CanvasRatio == ELCanvas.Ratio.iPad)
		{
			board.transform.localScale = new Vector3(0.8f, 0.8f, 1f);
			timeMeter.transform.localScale = new Vector3(0.8f, 0.8f, 1f);
		}
	}

	private void Update()
	{
		if (!ELSingleton<PopUpManager>.Instance.IsActiveAndEnabled() && UnityEngine.Input.GetKeyDown(KeyCode.Escape))
		{
			BackButton();
		}
		if (delay <= 0f)
		{
			switch (state)
			{
			case State.LoadNormal:
			case State.ReloadNormal:
				LoadLevel(LevelType.Normal);
				if (!isMusicPlaying)
				{
					ELSingleton<MusicManager>.Instance.PlayGameNormal();
					isMusicPlaying = true;
				}
				background.Reset(LevelType.Normal, pack.index);
				if (state == State.LoadNormal)
				{
					if (IsHowToPlayPopUp)
					{
						IsHowToPlayPopUp = false;
						ELSingleton<HowToPlayPopUp>.Instance.ShowPopUp(0.5f);
						ELSingleton<ApplicationSettings>.Instance.Save();
						SetState(State.Idle);
					}
					else
					{
						SetState(State.ShowLoadNormal);
					}
				}
				else
				{
					SetState(State.ShowReloadNormal);
				}
				ELSingleton<AnalyticsManager>.Instance.LevelStart((level.number + 1).ToString());
				ELSingleton<NotificationsManager>.Instance.RegisterNotifications(pack.index, level.index);
				break;
			case State.ShowLoadNormal:
				board.Enable(2.5f);
				board.ShowFill(2f);
				board.ShowWords(2.1f);
				board.ShowIcon(2.2f);
				board.wordText.Show(2.3f);
				if (!isLevelCompleted && background.HasLadyBug())
				{
					ladybug.Show();
				}
				else
				{
					ladybug.Reset();
				}
				levelText.Show(2.5f);
				categoryText.Show(0.2f);
				backButton.Enable(2.5f);
				backButton.Show(2.5f);
				shopButton.Enable(2.5f);
				shopButton.Show(2.5f);
				if (!level.isCompleted)
				{
					extraWordsButton.Enable(2.5f);
					extraWordsButton.Show(2.5f);
					perfectMeter.Show(2.5f);
				}
				hintBar.Set(level);
				if (ELSingleton<HintManager>.Instance.IsHintAvailable(LevelHint.GoodStart, level.number))
				{
					goodStartButton.Enable(2.5f);
					goodStartButton.Show(2.5f);
				}
				if (ELSingleton<HintManager>.Instance.IsHintAvailable(LevelHint.StartAndFinish, level.number))
				{
					startAndFinishButton.Enable(2.5f);
					startAndFinishButton.Show(2.5f);
				}
				if (ELSingleton<HintManager>.Instance.IsHintAvailable(LevelHint.Expose, level.number))
				{
					exposeButton.Enable(2.5f);
					exposeButton.Show(2.5f);
				}
				if (ELSingleton<HintManager>.Instance.IsHintAvailable(LevelHint.Tip, level.number))
				{
					tipButton.Enable(2.5f);
					tipButton.Show(2.5f);
				}
				SetState(State.Play, 2.5f);
				break;
			case State.ShowReloadNormal:
				board.Enable(2.5f);
				board.ShowFill(2f);
				board.ShowWords(2.1f);
				board.ShowIcon(2.2f);
				board.wordText.Show(2.3f);
				levelText.Show(2.5f);
				categoryText.Show(0.2f);
				backButton.Enable();
				shopButton.Enable(2.5f);
				if (!level.isCompleted)
				{
					extraWordsButton.Enable(2.5f);
					perfectMeter.Show(2.5f);
				}
				hintBar.Set(level, 2.2f);
				if (ELSingleton<HintManager>.Instance.IsHintAvailable(LevelHint.GoodStart, level.number))
				{
					goodStartButton.Enable(2.5f);
					goodStartButton.Show(2.5f);
				}
				if (ELSingleton<HintManager>.Instance.IsHintAvailable(LevelHint.StartAndFinish, level.number))
				{
					startAndFinishButton.Enable(2.5f);
					startAndFinishButton.Show(2.5f);
				}
				if (ELSingleton<HintManager>.Instance.IsHintAvailable(LevelHint.Expose, level.number))
				{
					exposeButton.Enable(2.5f);
					exposeButton.Show(2.5f);
				}
				if (ELSingleton<HintManager>.Instance.IsHintAvailable(LevelHint.Tip, level.number))
				{
					tipButton.Enable(2.5f);
					tipButton.Show(2.5f);
				}
				SetState(State.Play, 2.5f);
				break;
			case State.LoadBonusRound:
				LoadLevel(LevelType.BonusRound);
				if (!isMusicPlaying)
				{
					ELSingleton<MusicManager>.Instance.PlayGameSpecial();
					isMusicPlaying = true;
				}
				background.Reset(LevelType.BonusRound, -1);
				ladybug.Reset();
				levelText.Hide();
				categoryText.Hide();
				extraWordsButton.Disable();
				extraWordsButton.Hide();
				extraWordsButton.FullParticleStop();
				perfectMeter.Hide();
				goodStartButton.Disable();
				goodStartButton.Hide();
				startAndFinishButton.Disable();
				startAndFinishButton.Hide();
				exposeButton.Disable();
				exposeButton.Hide();
				tipButton.Disable();
				tipButton.Hide();
				tipText.Hide();
				specialWelcome.Show(LevelType.BonusRound, 0.5f);
				SetState(State.Idle);
				ELSingleton<AnalyticsManager>.Instance.LevelStart("bonus_round");
				break;
			case State.ShowBonusRound:
				board.Enable(1f);
				board.ShowFill(0.5f);
				board.ShowWords(0.6f);
				board.ShowIcon(0.7f);
				board.wordText.Show(0.8f);
				levelText.Show(1f);
				backButton.Enable(1f);
				backButton.Show(1f);
				shopButton.Enable(1f);
				shopButton.Show(1f);
				timeMeter.Show(1f);
				SetState(State.Play, 1f);
				break;
			case State.LoadDailyPuzzle:
				LoadLevel(LevelType.DailyPuzzle);
				if (!isMusicPlaying)
				{
					ELSingleton<MusicManager>.Instance.PlayGameSpecial();
					isMusicPlaying = true;
				}
				background.Reset(LevelType.DailyPuzzle, -2);
				ladybug.Reset();
				specialWelcome.Show(LevelType.DailyPuzzle, 0.5f);
				SetState(State.Idle);
				ELSingleton<AnalyticsManager>.Instance.LevelStart("daily_puzzle");
				break;
			case State.ShowDailyPuzzle:
				board.Enable(2.5f);
				board.ShowFill(2f);
				board.ShowWords(2.1f);
				board.ShowIcon(2.2f);
				board.wordText.Show(2.3f);
				levelText.Show(2.5f);
				categoryText.Show(0.2f);
				backButton.Enable(2.5f);
				backButton.Show(2.5f);
				shopButton.Enable(2.5f);
				shopButton.Show(2.5f);
				if (!level.isCompleted)
				{
					extraWordsButton.Enable(2.5f);
					extraWordsButton.Show(2.5f);
					perfectMeter.Show(2.5f);
				}
				hintBar.Set(level);
				if (ELSingleton<HintManager>.Instance.IsHintAvailable(LevelHint.GoodStart, level.number))
				{
					goodStartButton.Enable(2.5f);
					goodStartButton.Show(2.5f);
				}
				if (ELSingleton<HintManager>.Instance.IsHintAvailable(LevelHint.StartAndFinish, level.number))
				{
					startAndFinishButton.Enable(2.5f);
					startAndFinishButton.Show(2.5f);
				}
				if (ELSingleton<HintManager>.Instance.IsHintAvailable(LevelHint.Expose, level.number))
				{
					exposeButton.Enable(2.5f);
					exposeButton.Show(2.5f);
				}
				if (ELSingleton<HintManager>.Instance.IsHintAvailable(LevelHint.Tip, level.number))
				{
					tipButton.Enable(2.5f);
					tipButton.Show(2.5f);
				}
				SetState(State.Play, 2.5f);
				break;
			case State.Play:
				if (categoryText.IsShowEnded && Time.timeScale > 1f)
				{
					Time.timeScale = 1f;
				}
				if (!ELSingleton<PopUpManager>.Instance.IsActiveAndEnabled())
				{
					Tutorial tutorial = ELSingleton<TutorialManager>.Instance.CheckTrigger(level.type, level.number, level.stats.isPerfect && level.stats.valid > 0, board.wordText.IsDictionary, board.Result == Board.WordCheckResult.ExtraWord);
					if (tutorial != null)
					{
						ELSingleton<TutorialWindow>.Instance.Show(tutorial, (tutorial.type == TutorialType.GoodStart) ? (hintBar.transform.localPosition + goodStartButton.PositionTarget) : ((tutorial.type == TutorialType.StartAndFinish) ? (hintBar.transform.localPosition + startAndFinishButton.PositionTarget) : ((tutorial.type == TutorialType.Expose) ? (hintBar.transform.localPosition + exposeButton.PositionTarget) : ((tutorial.type == TutorialType.Tip) ? (hintBar.transform.localPosition + tipButton.PositionTarget) : ((tutorial.type == TutorialType.Perfect) ? perfectMeter.transform.localPosition : ((tutorial.type == TutorialType.Dictionary) ? (board.transform.localPosition + board.wordText.transform.localPosition * board.transform.localScale.x) : ((tutorial.type == TutorialType.Shop) ? shopButton.transform.localPosition : ((tutorial.type == TutorialType.ExtraWord) ? extraWordsButton.transform.localPosition : Vector3.zero))))))));
					}
				}
				if (level.type == LevelType.Normal && board.IsComplete())
				{
					board.Disable();
					backButton.Disable();
					shopButton.Disable();
					extraWordsButton.Disable();
					goodStartButton.Disable();
					startAndFinishButton.Disable();
					exposeButton.Disable();
					tipButton.Disable();
					tipText.Hide();
					if (!isLevelCompleted)
					{
						if (isLastLevelInPack)
						{
							ELSingleton<CoinsManager>.Instance.AddCoins(ELSingleton<XmlSettings>.Instance.coinsConfig.pack);
						}
						isPerfect = level.stats.isPerfect;
						if (level.stats.isPerfect)
						{
							ELSingleton<CoinsManager>.Instance.AddCoins(ELSingleton<XmlSettings>.Instance.coinsConfig.perfect);
						}
					}
					LevelStats levelStats = new LevelStats(level.stats);
					ELSingleton<LevelSuccessPopUp>.Instance.ShowPopUp(level.type, level.stats, isLastLevelInPack, isLevelCompleted, levelStats, board.HasCoins() ? 1.5f : 0.5f);
					ELSingleton<LevelsSettings>.Instance.levelSet.CompleteLevel(aIsCompleteLevel: true);
					ELSingleton<AnalyticsManager>.Instance.LevelComplete((level.number + 1).ToString(), new Dictionary<string, object>
					{
						{
							"extra_words",
							levelStats.pointsExtra
						}
					});
					SetState(State.Idle);
				}
				else if (level.type == LevelType.BonusRound && timeMeter.TimeCurrent <= 0f)
				{
					board.Disable();
					board.HideWords();
					board.wordText.Hide(0.3f);
					board.HideFill(0.5f);
					board.HideIcon(0.5f);
					shopButton.Disable();
					extraWordsButton.Disable();
					goodStartButton.Disable();
					startAndFinishButton.Disable();
					exposeButton.Disable();
					tipButton.Disable();
					tipText.Hide();
					LevelStats levelStats2 = new LevelStats(level.stats);
					ELSingleton<LevelSuccessPopUp>.Instance.ShowPopUp(level.type, level.stats, aIsLastLevelInPack: false, aIsLevelCompleted: false, levelStats2, 0.5f);
					ELSingleton<AnalyticsManager>.Instance.LevelComplete("bonus_round", new Dictionary<string, object>
					{
						{
							"extra_words",
							levelStats2.pointsExtra
						}
					});
					SetState(State.Idle);
				}
				else if (level.type == LevelType.DailyPuzzle && board.IsComplete())
				{
					board.Disable();
					board.HideWords();
					board.wordText.Hide(0.3f);
					board.HideFill(0.5f);
					board.HideIcon(0.5f);
					shopButton.Disable();
					extraWordsButton.Disable();
					goodStartButton.Disable();
					startAndFinishButton.Disable();
					exposeButton.Disable();
					tipButton.Disable();
					tipText.Hide();
					LevelStats levelStats3 = new LevelStats(level.stats);
					ELSingleton<LevelSuccessPopUp>.Instance.ShowPopUp(level.type, level.stats, aIsLastLevelInPack: false, aIsLevelCompleted: false, levelStats3, 0.5f);
					ELSingleton<AnalyticsManager>.Instance.LevelComplete("daily_puzzle", new Dictionary<string, object>
					{
						{
							"extra_words",
							levelStats3.pointsExtra
						}
					});
					SetState(State.Idle);
				}
				else if (board.IsWordValid)
				{
					if (level.type == LevelType.Normal || level.type == LevelType.DailyPuzzle)
					{
						switch (board.CheckWordNormal(level.isCompleted))
						{
						case Board.WordCheckResult.Valid:
							StatsAddValid();
							if (!isLevelCompleted)
							{
								StatsAddPointsNormal(1);
								ELSingleton<PointsManager>.Instance.AddPoints(1);
							}
							ELSingleton<AudioManager>.Instance.PlaySfx(soundWordValid);
							break;
						case Board.WordCheckResult.Invalid:
							if (board.wordText.Text.Length >= 3)
							{
								StatsSetPerfect(aIsPerfect: false);
							}
							ELSingleton<AudioManager>.Instance.PlaySfx(soundWordInvalid);
							break;
						case Board.WordCheckResult.Repeat:
							ELSingleton<AudioManager>.Instance.PlaySfx(soundWordRepeat);
							break;
						case Board.WordCheckResult.ExtraWord:
							if (!ELSingleton<ExtraWordsManager>.Instance.IsFull)
							{
								coinPod.ReleaseExtraWord(board.wordText.transform.position, extraWordsButton.coinTarget.transform.position, 2.5f);
								ELSingleton<ExtraWordsManager>.Instance.AddWords(1);
							}
							if (!isLevelCompleted)
							{
								StatsAddPointsExtra(1);
								ELSingleton<PointsManager>.Instance.AddPoints(1);
							}
							ELSingleton<AudioManager>.Instance.PlaySfx(soundWordExtra);
							break;
						}
					}
					else if (level.type == LevelType.BonusRound)
					{
						switch (board.CheckWordSpecial())
						{
						case Board.WordCheckResult.Valid:
							StatsAddValid();
							StatsAddPointsNormal(1);
							ELSingleton<PointsManager>.Instance.AddPoints(1);
							if (level.stats.valid % 4 == 0 && specialLevelRound > 0)
							{
								specialLevelRound--;
								board.SetSpecialLevelRound(specialLevelRound, aIsScaleForce: false);
								ELSingleton<AudioManager>.Instance.PlaySfx(soundSpecialLevelRound);
							}
							ELSingleton<AudioManager>.Instance.PlaySfx(soundWordValid);
							break;
						case Board.WordCheckResult.Invalid:
							ELSingleton<AudioManager>.Instance.PlaySfx(soundWordInvalid);
							break;
						}
					}
					board.IsWordValid = false;
				}
				if (level.type == LevelType.Normal || level.type == LevelType.DailyPuzzle)
				{
					perfectMeter.Setup();
				}
				if (level.type == LevelType.BonusRound)
				{
					timeMeter.Setup();
				}
				break;
			case State.ResetNormal:
				board.Reset();
				perfectMeter.Reset();
				if (isLastLevelInPack && !isLevelCompleted)
				{
					isMusicPlaying = false;
					SetState(State.LoadBonusRound);
				}
				else
				{
					SetState(State.ReloadNormal);
				}
				break;
			case State.HideBonusRound:
				board.wordText.Hide(0.3f);
				board.HideFill(0.5f);
				board.HideIcon(0.5f);
				categoryText.Hide(0.5f);
				backButton.Enable();
				timeMeter.Hide(0.5f);
				SetState(State.ResetBonusRound, 1f);
				break;
			case State.ResetBonusRound:
				board.Reset();
				specialWelcome.Reset();
				SetState(State.LoadBonusRound);
				break;
			case State.HideDailyPuzzle:
				board.wordText.Hide(0.3f);
				board.HideFill(0.5f);
				board.HideIcon(0.5f);
				categoryText.Hide(0.5f);
				backButton.Enable();
				extraWordsButton.Hide(0.5f);
				goodStartButton.Hide(0.5f);
				startAndFinishButton.Hide(0.5f);
				exposeButton.Hide(0.5f);
				tipButton.Hide(0.5f);
				tipText.Hide();
				perfectMeter.Hide(0.5f);
				SetState(State.ResetDailyPuzzle, 1f);
				break;
			case State.ResetDailyPuzzle:
				board.Reset();
				perfectMeter.Reset();
				specialWelcome.Reset();
				SetState(State.LoadDailyPuzzle);
				break;
			}
		}
		delay -= Time.deltaTime;
	}

	public new void ShowWindow()
	{
		base.ShowWindow();
		Reset(levelTypeLoad, packIndexLoad);
		Screen.sleepTimeout = -1;
	}

	public void SetLoad(LevelType aLevelType, int aPackIndex)
	{
		levelTypeLoad = aLevelType;
		packIndexLoad = aPackIndex;
		background.Reset(aLevelType, aPackIndex);
	}

	private void Reset(LevelType aLevelType, int aPackIndex)
	{
		background.Reset(aLevelType, aPackIndex);
		board.Reset();
		ladybug.Reset();
		levelText.Reset();
		categoryText.Reset();
		backButton.Reset();
		shopButton.Reset();
		extraWordsButton.Reset();
		perfectMeter.Reset();
		timeMeter.Reset();
		goodStartButton.Reset();
		startAndFinishButton.Reset();
		exposeButton.Reset();
		tipButton.Reset();
		tipText.Reset();
		specialWelcome.Reset();
		coinPod.Reset();
		level = null;
		pack = null;
		int num;
		switch (aLevelType)
		{
		default:
			num = 0;
			break;
		case LevelType.DailyPuzzle:
			num = 6;
			break;
		case LevelType.BonusRound:
			num = 4;
			break;
		case LevelType.Normal:
			num = 0;
			break;
		}
		state = (State)num;
		delay = 0f;
		isLevelCompleted = false;
		isLastLevelInPack = false;
		isPerfect = false;
		specialLevelRound = 0;
		isMusicPlaying = false;
		IsReplayAvailable = true;
	}

	private void LoadLevel(LevelType aLevelType)
	{
		switch (aLevelType)
		{
		case LevelType.Normal:
			level = ELSingleton<LevelsSettings>.Instance.levelSet.GetCurrentLevel();
			pack = ELSingleton<LevelsSettings>.Instance.levelSet.GetCurrentPack();
			board.Set(pack, level);
			levelText.Text = $"{pack.name}\n<color=#FFFFFF>Level {level.number + 1}</color>";
			categoryText.Text = level.name;
			perfectMeter.Set(level);
			isLevelCompleted = level.isCompleted;
			isLastLevelInPack = ELSingleton<LevelsSettings>.Instance.levelSet.IsLastLevelInPack();
			break;
		case LevelType.BonusRound:
			specialLevelRound = 3;
			level = ELSingleton<LevelsSettings>.Instance.levelSet.GetNextBonusRoundLevel(10);
			level.stats.Reset();
			level.index = -1;
			level.number = -1;
			pack = ELSingleton<LevelsSettings>.Instance.levelSet.GetCurrentPack();
			board.Set(pack, level);
			board.SetSpecialLevelRound(specialLevelRound, aIsScaleForce: true);
			levelText.Text = "<color=#FFFFFF>Bonus Round</color>";
			categoryText.Text = string.Empty;
			timeMeter.Set(60f);
			timeMeter.IsEnabled = true;
			isLevelCompleted = false;
			isLastLevelInPack = false;
			break;
		case LevelType.DailyPuzzle:
			level = ELSingleton<LevelsSettings>.Instance.levelSet.GetNextDailyPuzzleLevel(Random.Range(4, 6));
			level.stats.Reset();
			level.index = -2;
			level.number = -2;
			pack = ELSingleton<LevelsSettings>.Instance.levelSet.GetCurrentPack();
			board.Set(pack, level);
			levelText.Text = "<color=#FFFFFF>Daily Puzzle</color>";
			categoryText.Text = level.name;
			perfectMeter.Set(level);
			isLevelCompleted = false;
			isLastLevelInPack = false;
			break;
		}
		ELSingleton<TutorialManager>.Instance.Reset();
	}

	private void SetState(State aState, float aDelay = 0f)
	{
		state = aState;
		delay = aDelay;
	}

	public void StatsAddCoins(int aCoins)
	{
		level.stats.coins += aCoins;
		ELSingleton<LevelsSettings>.Instance.Save();
	}

	public void StatsAddPointsNormal(int aPoints)
	{
		if (!level.isCompleted)
		{
			level.stats.pointsNormal += aPoints;
			ELSingleton<LevelsSettings>.Instance.Save();
		}
	}

	public void StatsAddPointsExtra(int aPoints)
	{
		if (!level.isCompleted)
		{
			level.stats.pointsExtra += aPoints;
			ELSingleton<LevelsSettings>.Instance.Save();
		}
	}

	private void StatsSetPerfect(bool aIsPerfect)
	{
		level.stats.isPerfect = aIsPerfect;
		ELSingleton<LevelsSettings>.Instance.Save();
	}

	private void StatsAddValid()
	{
		level.stats.valid++;
	}

	public void BackButton()
	{
		if (backButton.IsEnabled)
		{
			backButton.Disable();
			shopButton.Disable();
			extraWordsButton.Disable();
			goodStartButton.Disable();
			startAndFinishButton.Disable();
			exposeButton.Disable();
			tipButton.Disable();
			if (level.type == LevelType.Normal)
			{
				ELSingleton<ApplicationManager>.Instance.ShowPackAfterGame();
			}
			else if (level.type == LevelType.BonusRound)
			{
				ELSingleton<ApplicationManager>.Instance.ShowPackCompleteAfterGame();
			}
			else if (level.type == LevelType.DailyPuzzle)
			{
				ELSingleton<ApplicationManager>.Instance.ShowMenuAfterGame();
			}
			ELSingleton<TutorialWindow>.Instance.HideEnded();
		}
	}

	public void ShopButton()
	{
		ELSingleton<ShopPopUp>.Instance.ShowPopUp();
		ELSingleton<TutorialWindow>.Instance.HideEnded();
	}

	public void ExtraWordsButton()
	{
		ELSingleton<ExtraWordsPopUp>.Instance.ShowPopUp();
		ELSingleton<TutorialWindow>.Instance.HideEnded();
	}

	private void HintButton(LevelHint aHintType)
	{
		if (state != State.Play || aHintType == LevelHint.None)
		{
			return;
		}
		if ((ELSingleton<HintManager>.Instance.GetAmount(aHintType) > 0 || (ELSingleton<HintManager>.Instance.GetCoins(aHintType) > 0 && ELSingleton<CoinsManager>.Instance.Coins >= ELSingleton<HintManager>.Instance.GetCoins(aHintType))) && board.Hint(aHintType))
		{
			if (ELSingleton<HintManager>.Instance.GetAmount(aHintType) > 0)
			{
				ELSingleton<HintManager>.Instance.ChangeAmount(aHintType, -1);
			}
			else
			{
				ELSingleton<CoinsManager>.Instance.AddCoins(-ELSingleton<HintManager>.Instance.GetCoins(aHintType));
				shopButton.AddCoins(-ELSingleton<HintManager>.Instance.GetCoins(aHintType));
			}
			ELSingleton<AudioManager>.Instance.PlaySfx(soundHint);
			AnalyticsManager instance = ELSingleton<AnalyticsManager>.Instance;
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			object value;
			switch (aHintType)
			{
			default:
				value = "none";
				break;
			case LevelHint.Tip:
				value = "tip";
				break;
			case LevelHint.Expose:
				value = "expose";
				break;
			case LevelHint.StartAndFinish:
				value = "start_and_finish";
				break;
			case LevelHint.GoodStart:
				value = "good_start";
				break;
			}
			dictionary.Add("type", value);
			dictionary.Add("level", level.number + 1);
			instance.Event("hint_used", dictionary);
		}
		else if (ELSingleton<HintManager>.Instance.GetAmount(aHintType) == 0 && (ELSingleton<HintManager>.Instance.GetCoins(aHintType) < 0 || ELSingleton<CoinsManager>.Instance.Coins < ELSingleton<HintManager>.Instance.GetCoins(aHintType)))
		{
			ShopButton();
			ELSingleton<AudioManager>.Instance.PlaySfx(soundButtonGeneral);
		}
		else
		{
			ELSingleton<AudioManager>.Instance.PlaySfx(soundButtonX);
		}
		goodStartButton.Setup();
		startAndFinishButton.Setup();
		exposeButton.Setup();
		tipButton.Setup();
		ELSingleton<TutorialWindow>.Instance.Hide();
	}

	public void GoodStartButton()
	{
		HintButton(LevelHint.GoodStart);
	}

	public void StartAndFinishButton()
	{
		HintButton(LevelHint.StartAndFinish);
	}

	public void ExposeButton()
	{
		HintButton(LevelHint.Expose);
	}

	public void TipButton()
	{
		HintButton(LevelHint.Tip);
	}

	public void HowToPlayExitButton()
	{
		if (base.isActiveAndEnabled && state == State.Idle)
		{
			SetState(State.ShowLoadNormal);
		}
	}

	public void SpecialWelcomePlayButton()
	{
		specialWelcome.playButton.Disable();
		specialWelcome.Hide();
		if (level.type == LevelType.BonusRound)
		{
			SetState(State.ShowBonusRound, 0.5f);
		}
		else if (level.type == LevelType.DailyPuzzle)
		{
			SetState(State.ShowDailyPuzzle, 0.5f);
		}
	}

	public void LevelSuccessPopUpShowHasEnded()
	{
		board.wordText.Hide();
		board.HideFill();
		board.HideIcon();
		categoryText.Hide();
		backButton.Enable();
		perfectMeter.Hide();
	}

	public void LevelSuccessPopUpNextLevelButton()
	{
		if (!ELSingleton<AdsManager>.Instance.ShowInterstitialAd(pack.index, level.index))
		{
			LevelSuccessPopUpNextLevelButtonExecute();
		}
	}

	public void LevelSuccessPopUpNextLevelButtonExecute()
	{
		if (!isLevelCompleted && isPerfect)
		{
			coinPod.ReleaseCoinsRadial(ELSingleton<XmlSettings>.Instance.coinsConfig.perfect, ELSingleton<LevelSuccessPopUp>.Instance.perfect.transform.position, shopButton.coinTarget.transform.position, 1f, 1.5f, 1.5f, 0.3f, aIsFullUpdate: false, aIsWithParticles: false);
		}
		ELSingleton<LevelSuccessPopUp>.Instance.HidePopUp();
		ELSingleton<LevelsSettings>.Instance.levelSet.SetNextLevel();
		SetState(State.ResetNormal, 0.5f);
	}

	public void LevelSuccessPopUpContinueButton()
	{
		ELSingleton<LevelSuccessPopUp>.Instance.HidePopUp();
		if (level.type == LevelType.BonusRound)
		{
			ELSingleton<ApplicationManager>.Instance.ShowPackCompleteAfterGame();
		}
		else if (level.type == LevelType.DailyPuzzle)
		{
			ELSingleton<ApplicationManager>.Instance.ShowMenuAfterGame();
		}
	}

	public void LevelSuccessPopUpReplayButton()
	{
		if (IsReplayAvailable && !ELSingleton<AdsManager>.Instance.ShowRewardedVideoAd((level.type == LevelType.BonusRound) ? AdsManager.AdType.BonusRoundReplay : AdsManager.AdType.DailyPuzzleReplay, aIsExecute: true))
		{
			LevelSuccessPopUpReplayButtonExecute();
		}
		IsReplayAvailable = false;
	}

	public void LevelSuccessPopUpReplayButtonExecute()
	{
		ELSingleton<LevelSuccessPopUp>.Instance.HidePopUp();
		if (level.type == LevelType.BonusRound)
		{
			SetState(State.HideBonusRound, 0.5f);
		}
		else if (level.type == LevelType.DailyPuzzle)
		{
			SetState(State.HideDailyPuzzle, 0.5f);
		}
	}

	public void PopUpWillShow()
	{
		if (base.isActiveAndEnabled)
		{
			background.StopAnimation();
			if (state == State.Play)
			{
				board.Disable();
			}
			ladybug.StopAnimation();
			levelText.StopAnimation();
			categoryText.StopAnimation();
			backButton.StopAnimation();
			shopButton.StopAnimation();
			extraWordsButton.StopAnimation();
			goodStartButton.StopAnimation();
			startAndFinishButton.StopAnimation();
			exposeButton.StopAnimation();
			tipButton.StopAnimation();
		}
	}

	public void PopUpHasHidden()
	{
		if (base.isActiveAndEnabled && !ELSingleton<PopUpManager>.Instance.IsActiveAndEnabled())
		{
			background.StartAnimation();
			if (state == State.Play)
			{
				board.Enable();
			}
			ladybug.StartAnimation();
			levelText.StartAnimation();
			categoryText.StartAnimation();
			backButton.StartAnimation();
			shopButton.StartAnimation();
			extraWordsButton.StartAnimation();
			goodStartButton.StartAnimation();
			startAndFinishButton.StartAnimation();
			exposeButton.StartAnimation();
			tipButton.StartAnimation();
		}
	}

	public void WordTextDictionary()
	{
		if (board.wordText.IsDictionary)
		{
			ELSingleton<DictPopUp>.Instance.ShowPopUp(board.wordText.Text);
			ELSingleton<TutorialWindow>.Instance.HideEnded();
		}
	}
}
