using UnityEngine;

public class AdsManager : ELSingleton<AdsManager>
{
	public enum AdType
	{
		None,
		Interstitial,
		BonusRoundReplay,
		DailyPuzzleReplay,
		FreeCoins
	}

	private string interstitialAdId = "InterstitialAd";

	private const string rewardedVideoAdId = "RewardedVideoAd";

	public bool isDebug;

	private AdType adType;

	public bool IsTurnedOn
	{
		get;
		set;
	}

	public bool IsInterstitialAd
	{
		get;
		private set;
	}

	public bool IsRewardedVideoAd
	{
		get;
		private set;
	}

	private void Awake()
	{
		IsTurnedOn = true;
		IsInterstitialAd = false;
		IsRewardedVideoAd = false;
		adType = AdType.None;
	}

	private void Start()
	{
		AATKitConfiguration configuration = new AATKitConfiguration
		{
			SimpleConsent = AATKitConfiguration.Consent.OBTAINED,
			ConsentRequired = ELSingleton<GDPRManager>.Instance.IsGDPR()
		};
		AATKitBinding.Init(base.gameObject.name, configuration);
		AATKitBinding.CreatePlacement(interstitialAdId, AATKitBinding.PlacementSize.Fullscreen);
		AATKitBinding.StartPlacementAutoReload(interstitialAdId);
		AATKitBinding.CreatePlacement("RewardedVideoAd", AATKitBinding.PlacementSize.Rewarded);
		AATKitBinding.StartPlacementAutoReload("RewardedVideoAd");
	}

	public void OnHaveAd(string aPlacementName)
	{
		if (isDebug)
		{
			UnityEngine.Debug.Log("AdsManager_OnHaveAd: " + aPlacementName);
		}
		if (aPlacementName != null)
		{
			if (aPlacementName.Equals(interstitialAdId))
			{
				IsInterstitialAd = true;
			}
			else if (aPlacementName.Equals("RewardedVideoAd"))
			{
				IsRewardedVideoAd = true;
			}
		}
	}

	public void OnHaveAdOnMultiSizeBanner(string aPlacementName)
	{
		if (isDebug)
		{
			UnityEngine.Debug.Log("AdsManager_OnHaveAdOnMultiSizeBanner: " + aPlacementName);
		}
	}

	public void OnNoAd(string aPlacementName)
	{
		if (isDebug)
		{
			UnityEngine.Debug.Log("AdsManager_OnNoAd: " + aPlacementName);
		}
		if (aPlacementName != null)
		{
			if (aPlacementName.Equals(interstitialAdId))
			{
				IsInterstitialAd = false;
			}
			else if (aPlacementName.Equals("RewardedVideoAd"))
			{
				IsRewardedVideoAd = false;
			}
		}
	}

	public void OnPauseForAd(string aPlacementName)
	{
		if (isDebug)
		{
			UnityEngine.Debug.Log("AdsManager_OnPauseForAd: " + aPlacementName);
		}
	}

	public void OnResumeAfterAd(string aPlacementName)
	{
		if (isDebug)
		{
			UnityEngine.Debug.Log("AdsManager_OnResumeAfterAd: " + aPlacementName);
		}
		ELSingleton<AudioManager>.Instance.ResumeMusic();
		if (adType == AdType.Interstitial)
		{
			ELSingleton<GameWindow>.Instance.LevelSuccessPopUpNextLevelButtonExecute();
		}
	}

	public void OnShowingEmpty(string aPlacementName)
	{
		if (isDebug)
		{
			UnityEngine.Debug.Log("AdsManager_OnShowingEmpty: " + aPlacementName);
		}
	}

	public void OnUserEarnedIncentive(string aPlacementName)
	{
		if (isDebug)
		{
			UnityEngine.Debug.Log("AdsManager_OnUserEarnedIncentive: " + aPlacementName);
		}
		RewardedVideoAdReward(adType);
	}

	public void OnObtainedAdRules(string aFromTheServer)
	{
		if (isDebug)
		{
			UnityEngine.Debug.Log("AdsManager_OnObtainedAdRules: " + aFromTheServer);
		}
	}

	public void OnOrientationChange(string aOrientation)
	{
	}

	public bool ShowInterstitialAd(int aPackIndex, int aLevelIndex)
	{
		bool flag = false;
		for (int i = 0; i < ELSingleton<XmlSettings>.Instance.interstitialAdsConfig.Count; i++)
		{
			int pack = ELSingleton<XmlSettings>.Instance.interstitialAdsConfig[i].pack;
			int level = ELSingleton<XmlSettings>.Instance.interstitialAdsConfig[i].level;
			if ((pack == -1 || pack - 1 == aPackIndex) && (level == -1 || level - 1 == aLevelIndex))
			{
				flag = true;
				break;
			}
		}
		if ((IsTurnedOn && IsInterstitialAd) & flag)
		{
			adType = AdType.Interstitial;
			ELSingleton<AudioManager>.Instance.PauseMusic();
			AATKitBinding.ShowPlacement(interstitialAdId);
			return true;
		}
		return false;
	}

	public bool ShowRewardedVideoAd(AdType aAdType, bool aIsExecute)
	{
		if (IsRewardedVideoAd)
		{
			if (aIsExecute)
			{
				adType = aAdType;
				ELSingleton<AudioManager>.Instance.PauseMusic();
				AATKitBinding.ShowPlacement("RewardedVideoAd");
			}
			return true;
		}
		return false;
	}

	private void RewardedVideoAdReward(AdType aAdType)
	{
		switch (aAdType)
		{
		case AdType.BonusRoundReplay:
			ELSingleton<GameWindow>.Instance.LevelSuccessPopUpReplayButtonExecute();
			break;
		case AdType.DailyPuzzleReplay:
			ELSingleton<GameWindow>.Instance.LevelSuccessPopUpReplayButtonExecute();
			break;
		case AdType.FreeCoins:
		{
			ELSingleton<CoinsManager>.Instance.AddCoins(ELSingleton<XmlSettings>.Instance.rewardedVideoAdConfig.coins);
			XmlSettings.IapConfig iapConfig = new XmlSettings.IapConfig();
			iapConfig.coins = ELSingleton<XmlSettings>.Instance.rewardedVideoAdConfig.coins;
			ELSingleton<IapManager>.Instance.CollectItems(iapConfig);
			if (ELSingleton<ShopPopUp>.Instance.isActiveAndEnabled)
			{
				ELSingleton<ShopPopUp>.Instance.HidePopUp();
			}
			break;
		}
		}
	}

	public void TurnOff()
	{
		IsTurnedOn = false;
		ELSingleton<ApplicationSettings>.Instance.Save();
	}
}
