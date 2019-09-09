using System.Collections.Generic;
using UnityEngine;

public class AATKitDemo : MonoBehaviour
{
	public enum DemoState
	{
		Init,
		Banner,
		Fullscreen,
		Multisize
	}

	private DemoState currentState;

	private string version = "unknown";

	private const string bannerPlacement = "BannerPlacement";

	private const string interstitialPlacement = "FullscreenPlacement";

	private const string multiSizePlacement = "MultiSizePlacement";

	private const string nativePlacement = "NativePlacement";

	private bool initialized;

	private bool shakeEnabled;

	private AATKitBinding.BannerAlignment bannerAlignment = AATKitBinding.BannerAlignment.BottomCenter;

	private AATKitBinding.BannerAlignment multiSizeBannerAlignment = AATKitBinding.BannerAlignment.BottomCenter;

	private bool bannerAutoreload;

	private bool multiSizeAutoreload;

	private bool interstitialAutoreload;

	private int buttonWidth = 256;

	private int buttonHeight = 64;

	private string shakeText = "Enable Debug Shake";

	private string autoreloadBannerText = "Enable Banner Autoreload";

	private string autoreloadMultiSizeText = "Enable Multi Size Banner Autoreload";

	private string autoreloadInterstitialText = "Enable Interstitial Autoreload";

	private void Start()
	{
		version = AATKitBinding.GetVersion();
	}

	private void OnGUI()
	{
		GUI.skin.label.fontSize = 40;
		GUI.skin.button.fontSize = 30;
		buttonWidth = Screen.width / 2;
		buttonHeight = buttonWidth / 4;
		if (GUI.Button(new Rect(10f, 10f, 200f, 100f), "BACK"))
		{
			currentState = DemoState.Init;
		}
		GUI.Label(new Rect(250f, 35f, Screen.width - 260, 45f), "AATKit Unity " + version);
		if (currentState == DemoState.Init)
		{
			if (GUI.Button(new Rect(Screen.width / 2 - buttonWidth / 2, 140f, buttonWidth, buttonHeight), "Init AATKit") && !initialized)
			{
				AATKitConfiguration configuration = new AATKitConfiguration
				{
					TestModeAccountId = 74,
					DetailedConsent = AATKitConfiguration.DetailedConsentTypes.ManagedConsent,
					ManagedConsentLanguages = new List<AATKitConfiguration.ManagedConsentLanguage>
					{
						AATKitConfiguration.ManagedConsentLanguage.ENGLISH,
						AATKitConfiguration.ManagedConsentLanguage.GERMAN
					}
				};
				AATKitBinding.Init(base.gameObject.name, configuration);
				AATKitBinding.SetDebugEnabled();
				Vector2 vector = AATKitBinding.CreatePlacement("BannerPlacement", AATKitBinding.PlacementSize.BannerAuto);
				Vector2 vector2 = AATKitBinding.CreatePlacement("FullscreenPlacement", AATKitBinding.PlacementSize.Fullscreen);
				AATKitBinding.CreatePlacement("MultiSizePlacement", AATKitBinding.PlacementSize.MultiSizeBanner);
				UnityEngine.Debug.Log("Banner size: " + vector.x + " x " + vector.y);
				UnityEngine.Debug.Log("Fullscreen size: " + vector2.x + " x " + vector2.y);
				initialized = true;
			}
			if (GUI.Button(new Rect(Screen.width / 2 - buttonWidth / 2, buttonHeight + 150, buttonWidth, buttonHeight), "Enable Debug Log") && initialized)
			{
				AATKitBinding.SetDebugEnabled();
			}
			if (GUI.Button(new Rect(Screen.width / 2 - buttonWidth / 2, buttonHeight * 2 + 160, buttonWidth, buttonHeight), shakeText) && initialized)
			{
				if (shakeEnabled)
				{
					AATKitBinding.SetDebugShakeEnabled(enabled: false);
					shakeEnabled = false;
					shakeText = "Enable Debug Shake";
				}
				else
				{
					AATKitBinding.SetDebugShakeEnabled(enabled: true);
					shakeEnabled = true;
					shakeText = "Disable Debug Shake";
				}
			}
			if (GUI.Button(new Rect(Screen.width / 2 - buttonWidth / 2, buttonHeight * 3 + 200, buttonWidth, buttonHeight), "BANNER"))
			{
				currentState = DemoState.Banner;
			}
			if (GUI.Button(new Rect(Screen.width / 2 - buttonWidth / 2, buttonHeight * 4 + 220, buttonWidth, buttonHeight), "FULLSCREEN"))
			{
				currentState = DemoState.Fullscreen;
			}
			if (GUI.Button(new Rect(Screen.width / 2 - buttonWidth / 2, buttonHeight * 5 + 230, buttonWidth, buttonHeight), "MULTISIZE"))
			{
				currentState = DemoState.Multisize;
			}
		}
		if (currentState == DemoState.Banner)
		{
			if (GUI.Button(new Rect(Screen.width / 2 - buttonWidth / 2, 140f, buttonWidth, buttonHeight), "Reload Banner") && initialized)
			{
				AATKitBinding.ReloadPlacement("BannerPlacement");
			}
			if (GUI.Button(new Rect(Screen.width / 2 - buttonWidth / 2, buttonHeight + 150, buttonWidth, buttonHeight), autoreloadBannerText) && initialized)
			{
				if (bannerAutoreload)
				{
					AATKitBinding.StopPlacementAutoReload("BannerPlacement");
					bannerAutoreload = false;
					autoreloadBannerText = "Enable Banner Autoreload";
				}
				else
				{
					AATKitBinding.StartPlacementAutoReload("BannerPlacement");
					bannerAutoreload = true;
					autoreloadBannerText = "Disable Banner Autoreload";
				}
			}
			if (GUI.Button(new Rect(Screen.width / 2 - buttonWidth / 2, buttonHeight * 2 + 160, buttonWidth, buttonHeight), "Change Banner Alignment") && initialized)
			{
				bannerAlignment = GetNewAlignment(bannerAlignment);
				AATKitBinding.SetPlacementAlignment("BannerPlacement", bannerAlignment);
			}
		}
		if (currentState == DemoState.Fullscreen)
		{
			if (GUI.Button(new Rect(Screen.width / 2 - buttonWidth / 2, 140f, buttonWidth, buttonHeight), "Reload Interstitial") && initialized)
			{
				AATKitBinding.ReloadPlacement("FullscreenPlacement");
			}
			if (GUI.Button(new Rect(Screen.width / 2 - buttonWidth / 2, buttonHeight + 150, buttonWidth, buttonHeight), autoreloadInterstitialText) && initialized)
			{
				if (interstitialAutoreload)
				{
					AATKitBinding.StopPlacementAutoReload("FullscreenPlacement");
					interstitialAutoreload = false;
					autoreloadInterstitialText = "Enable Interstitial Autoreload";
				}
				else
				{
					AATKitBinding.StartPlacementAutoReload("FullscreenPlacement");
					interstitialAutoreload = true;
					autoreloadInterstitialText = "Disable Interstitial Autoreload";
				}
			}
			if (GUI.Button(new Rect(Screen.width / 2 - buttonWidth / 2, buttonHeight * 2 + 160, buttonWidth, buttonHeight), "Show Interstitial") && initialized)
			{
				AATKitBinding.ShowPlacement("FullscreenPlacement");
			}
		}
		if (currentState != DemoState.Multisize)
		{
			return;
		}
		if (GUI.Button(new Rect(Screen.width / 2 - buttonWidth / 2, 140f, buttonWidth, buttonHeight), "Reload MultiSize") && initialized)
		{
			AATKitBinding.ReloadPlacement("MultiSizePlacement");
		}
		if (GUI.Button(new Rect(Screen.width / 2 - buttonWidth / 2, buttonHeight + 150, buttonWidth, buttonHeight), autoreloadMultiSizeText) && initialized)
		{
			if (multiSizeAutoreload)
			{
				AATKitBinding.StopPlacementAutoReload("MultiSizePlacement");
				multiSizeAutoreload = false;
				autoreloadMultiSizeText = "Enable Multi Size Banner Autoreload";
			}
			else
			{
				AATKitBinding.StartPlacementAutoReload("MultiSizePlacement");
				multiSizeAutoreload = true;
				autoreloadMultiSizeText = "Disable Multi Size Banner Autoreload";
			}
		}
		if (GUI.Button(new Rect(Screen.width / 2 - buttonWidth / 2, buttonHeight * 2 + 160, buttonWidth, buttonHeight), "Change MultiSize Alignment") && initialized)
		{
			multiSizeBannerAlignment = GetNewAlignment(multiSizeBannerAlignment);
			AATKitBinding.SetMultiSizeAlignment("MultiSizePlacement", multiSizeBannerAlignment);
		}
	}

	private AATKitBinding.BannerAlignment GetNewAlignment(AATKitBinding.BannerAlignment alignment)
	{
		AATKitBinding.BannerAlignment result = alignment;
		switch (alignment)
		{
		case AATKitBinding.BannerAlignment.BottomCenter:
			result = AATKitBinding.BannerAlignment.BottomRight;
			break;
		case AATKitBinding.BannerAlignment.BottomRight:
			result = AATKitBinding.BannerAlignment.BottomLeft;
			break;
		case AATKitBinding.BannerAlignment.BottomLeft:
			result = AATKitBinding.BannerAlignment.TopCenter;
			break;
		case AATKitBinding.BannerAlignment.TopCenter:
			result = AATKitBinding.BannerAlignment.TopRight;
			break;
		case AATKitBinding.BannerAlignment.TopRight:
			result = AATKitBinding.BannerAlignment.TopLeft;
			break;
		case AATKitBinding.BannerAlignment.TopLeft:
			result = AATKitBinding.BannerAlignment.BottomCenter;
			break;
		}
		return result;
	}

	public void OnHaveAd(string placementName)
	{
		UnityEngine.Debug.Log("onHaveAd event: " + placementName);
	}

	public void OnHaveAdOnMultiSizeBanner(string placementName)
	{
		UnityEngine.Debug.Log("onHaveAdOnMultiSizeBanner event: " + placementName);
	}

	public void OnNoAd(string placementName)
	{
		UnityEngine.Debug.Log("onNoAd event: " + placementName);
	}

	public void OnPauseForAd(string placementName)
	{
		UnityEngine.Debug.Log("onPauseForAd event: " + placementName);
	}

	public void OnResumeAfterAd(string placementName)
	{
		UnityEngine.Debug.Log("onResumeAfterAd event: " + placementName);
	}

	public void OnShowingEmpty(string placementName)
	{
		UnityEngine.Debug.Log("onShowingEmpty event: " + placementName);
	}

	public void OnUserEarnedIncentive(string placementName)
	{
		UnityEngine.Debug.Log("onUserEarnedIncentive event: " + placementName);
	}

	public void OnObtainedAdRules(string fromTheServer)
	{
		UnityEngine.Debug.Log("onObtainedAdRules event: " + fromTheServer);
	}

	public void OnManagedConsentNeedsUserInterface()
	{
		UnityEngine.Debug.Log("OnManagedConsentNeedsUserInterface");
		AATKitBinding.ShowConsentDialogIfNeeded();
	}

	public void OnManagedConsentCompletion()
	{
		UnityEngine.Debug.Log("OnManagedConsentCompletion");
	}
}
