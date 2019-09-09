using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuWindow : ELWindow<MenuWindow>
{
	public CommonButton dailyBonusButton;

	public Text versionText;

	public StickerMainMenu stickerPrefab;

	public GameObject stickerPad;

	public ShopButton shopButton;

	public CommonCoinPod coinPod;

	public Transform coinTarget;

	public GameObject shareCoins;

	public Text shareCoinsText;

	public GameObject rateUsCoins;

	public Text rateUsCoinsText;

	private bool isDoubleClick;

	public CommonButton fbButton;

	public CommonButton fbAvatar;

	public RectTransform wordsRectTransform;

	public TextMeshProUGUI wordsText;

	public Image dailyBonusOn;

	public Image dailyBonusOff;

	public Text dailyBonusTimeText;

	public ParticleSystem dailyBonusParticle;

	public RectTransform centerSection;

	public GameObject settingsButton;

	public GameObject rateButton;

	public GameObject shareButton;

	private void Awake()
	{
		isDoubleClick = false;
	}

	private new void Start()
	{
		base.Start();
		if (ELDevice.HasNotch())
		{
			shopButton.transform.localPosition = new Vector3(shopButton.transform.localPosition.x, shopButton.transform.localPosition.y - ELDevice.notchOffsetY, 0f);
			fbButton.transform.localPosition = new Vector3(fbButton.transform.localPosition.x, fbButton.transform.localPosition.y - ELDevice.notchOffsetY, 0f);
			fbAvatar.transform.localPosition = new Vector3(fbAvatar.transform.localPosition.x, fbAvatar.transform.localPosition.y - ELDevice.notchOffsetY, 0f);
			settingsButton.transform.localPosition = new Vector3(settingsButton.transform.localPosition.x, settingsButton.transform.localPosition.y + ELDevice.notchOffsetY, 0f);
			rateButton.transform.localPosition = new Vector3(rateButton.transform.localPosition.x, rateButton.transform.localPosition.y + ELDevice.notchOffsetY, 0f);
			shareButton.transform.localPosition = new Vector3(shareButton.transform.localPosition.x, shareButton.transform.localPosition.y + ELDevice.notchOffsetY, 0f);
		}
	}

	private void Update()
	{
		if (UnityEngine.Input.touchCount > 0)
		{
			isDoubleClick = false;
		}
		if (!ELSingleton<PopUpManager>.Instance.IsActiveAndEnabled() && UnityEngine.Input.GetKeyDown(KeyCode.Escape))
		{
			if (isDoubleClick)
			{
				Application.Quit();
			}
			isDoubleClick = true;
		}
		if (base.isActiveAndEnabled)
		{
			bool isAvailable = ELSingleton<DailyPuzzleManager>.Instance.IsAvailable;
			dailyBonusOn.gameObject.SetActive(isAvailable);
			dailyBonusOff.gameObject.SetActive(!isAvailable);
			if (isAvailable)
			{
				dailyBonusTimeText.text = "";
				dailyBonusParticle.gameObject.SetActive(dailyBonusButton.IsShown);
			}
			else
			{
				dailyBonusTimeText.text = ELSingleton<DailyPuzzleManager>.Instance.GetTimeSpanString();
				dailyBonusParticle.gameObject.SetActive(value: false);
			}
			if (fbAvatar.isActiveAndEnabled)
			{
				wordsRectTransform.transform.localPosition = fbAvatar.transform.localPosition + new Vector3(0f, -120f);
			}
			if (fbButton.isActiveAndEnabled)
			{
				wordsRectTransform.transform.localPosition = fbButton.transform.localPosition + new Vector3(0f, -120f);
			}
		}
	}

	public void ShareUpdate()
	{
		if (base.isActiveAndEnabled)
		{
			shareCoins.SetActive(ELSingleton<ShareManager>.Instance.HasCoinsReward());
			shareCoinsText.gameObject.SetActive(ELSingleton<ShareManager>.Instance.HasCoinsReward());
			shareCoinsText.text = "+" + ELSingleton<XmlSettings>.Instance.shareConfig.coins;
		}
	}

	public void RateUsUpdate()
	{
		if (base.isActiveAndEnabled)
		{
			rateUsCoins.SetActive(!ELSingleton<RateUsManager>.Instance.consumed);
			rateUsCoinsText.gameObject.SetActive(!ELSingleton<RateUsManager>.Instance.consumed);
			rateUsCoinsText.text = "+" + ELSingleton<XmlSettings>.Instance.rateItConfig.coins;
		}
	}

	public new void ShowWindow()
	{
		foreach (Transform item in stickerPad.transform)
		{
			item.gameObject.SetActive(value: false);
			UnityEngine.Object.Destroy(item.gameObject);
		}
		StickerMainMenu.Reset();
		for (int i = 0; i < 20; i++)
		{
			Object.Instantiate(stickerPrefab, stickerPad.transform);
		}
		base.ShowWindow();
		shopButton.Reset();
		dailyBonusButton.Reset();
		ShareUpdate();
		RateUsUpdate();
		if (ELSingleton<ELCanvas>.Instance.CanvasRatio == ELCanvas.Ratio.iPad)
		{
			centerSection.localScale = new Vector3(0.8f, 0.8f, 0.8f);
		}
		if (ELSingleton<ELCanvas>.Instance.CanvasRatio == ELCanvas.Ratio.iPhone)
		{
			centerSection.localScale = new Vector3(0.9f, 0.9f, 0.9f);
		}
		wordsText.text = string.Concat(ELSingleton<PointsManager>.Instance.Points);
		float num = 0f;
		Component[] componentsInChildren = base.gameObject.GetComponentsInChildren<CommonButton>();
		componentsInChildren = componentsInChildren;
		for (int j = 0; j < componentsInChildren.Length; j++)
		{
			((CommonButton)componentsInChildren[j]).Show(num);
			num += 0.1f;
		}
		if (ELSingleton<ApplicationSettings>.Instance.DeploymentSettings.isDebug)
		{
			versionText.text = $"v{Application.version} {ELSingleton<ELCanvas>.Instance.CanvasRatio}";
		}
		else
		{
			versionText.gameObject.SetActive(value: false);
		}
		Screen.sleepTimeout = -2;
	}

	public void PopUps()
	{
		Invoke("PopUpsInvoke", 0f);
	}

	public void PopUpsInvoke()
	{
		if (ELSingleton<FacebookManager>.Instance.connected)
		{
			ELSingleton<FacebookManager>.Instance.connectedAction(this, "FBConnectCallback");
		}
	}

	public void ShopButton()
	{
		ELSingleton<ShopPopUp>.Instance.ShowPopUp();
	}

	public void PlayButton()
	{
		LevelInfo firstNotCompleateLevel = ELSingleton<LevelsSettings>.Instance.levelSet.GetFirstNotCompleateLevel();
		if (ELSingleton<LevelsSettings>.Instance.levelSet.IsGameCompleted(firstNotCompleateLevel))
		{
			ELSingleton<ApplicationManager>.Instance.ShowPackAfterMenu();
			return;
		}
		ELSingleton<LevelsSettings>.Instance.levelSet.SetCurrentLevel(firstNotCompleateLevel);
		ELSingleton<GameWindow>.Instance.SetLoad(LevelType.Normal, ELSingleton<LevelsSettings>.Instance.levelSet.GetCurrentPack().index);
		ELSingleton<ApplicationManager>.Instance.ShowGameAfterMenu();
	}

	public void StickersButton()
	{
		ELSingleton<ApplicationManager>.Instance.ShowPackAfterMenu();
	}

	public void DailyBonusButton()
	{
		if (ELSingleton<DailyPuzzleManager>.Instance.IsAvailable)
		{
			ELSingleton<GameWindow>.Instance.SetLoad(LevelType.DailyPuzzle, -1);
			ELSingleton<ApplicationManager>.Instance.ShowGameAfterMenu();
			ELSingleton<DailyPuzzleManager>.Instance.Consume();
		}
	}

	public void SettingsButton()
	{
		ELSingleton<SettingsPopUp>.Instance.ShowPopUp();
	}

	public void RateUsButton()
	{
		ELSingleton<RateUsPopUp>.Instance.ShowPopUp();
	}

	public void ShareButton()
	{
		ELSingleton<SharePopUp>.Instance.ShowPopUp();
	}

	public void FBButton()
	{
		ELSingleton<FacebookPopUp>.Instance.ShowPopUp();
	}

	public void FBAvatarButton()
	{
		ELSingleton<FBRankingPopUpPopUp>.Instance.ShowPopUp();
	}

	public void FBConnectCallback()
	{
		if (ELSingleton<FacebookPopUp>.Instance.isActiveAndEnabled)
		{
			ELSingleton<FacebookPopUp>.Instance.HidePopUp();
		}
		if (ELSingleton<FacebookManager>.Instance.lastError != null && ELSingleton<FacebookManager>.Instance.lastError.Length > 0)
		{
			ELSingleton<MessagePopUp>.Instance.ShowPopUp(null, ELSingleton<FacebookManager>.Instance.lastError, 0.5f);
			return;
		}
		fbButton.gameObject.SetActive(value: false);
		fbAvatar.gameObject.SetActive(value: true);
		fbAvatar.Show();
		wordsText.text = string.Concat(ELSingleton<PointsManager>.Instance.Points);
		ELSingleton<FacebookManager>.Instance.FBSet();
	}

	public void FBDisconnect()
	{
		fbButton.gameObject.SetActive(value: true);
		fbButton.Show();
		fbAvatar.gameObject.SetActive(value: false);
	}
}
