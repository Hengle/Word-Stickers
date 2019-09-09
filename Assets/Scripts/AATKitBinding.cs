using System;
using System.Collections.Generic;
using UnityEngine;

public class AATKitBinding : MonoBehaviour
{
	public enum PlacementSize
	{
		Banner320x53 = 0,
		Banner768x90 = 1,
		Banner300x250 = 2,
		Fullscreen = 3,
		MultiSizeBanner = 6,
		Rewarded = 7,
		BannerAuto = 10
	}

	public enum BannerAlignment
	{
		TopLeft,
		TopCenter,
		TopRight,
		BottomLeft,
		BottomCenter,
		BottomRight
	}

	public enum AdNetwork
	{
		HOUSE,
		INMOBI,
		ADMOB,
		EMPTY,
		APPLOVIN,
		SMARTAD,
		ADX,
		DFP,
		SMAATO,
		FACEBOOK,
		UNITYADS,
		ADCOLONY,
		LOOPME,
		MOPUB,
		FLURRY,
		PERMODO,
		APPNEXUS,
		INNERACTIVE,
		APPNEXUSHBMOPUB,
		APPNEXUSHBDFP,
		OGURY,
		CRITEO,
		SPOTX,
		GENERICVAST,
		ONEBYAOL,
		THIRDPRESENCE,
		VUNGLE,
		RUBICON
	}

	public enum NativeAdType
	{
		APP_INSTALL,
		CONTENT,
		OTHER,
		UNKNOWN,
		VIDEO
	}

	public enum PlacementContentGravity
	{
		Top,
		Bottom,
		Center
	}

	public static readonly string Version = "2.13.9";

	private static readonly string LogPrefix = "[AATKitBinding] ";

	private static readonly string CSVSeparator = ";";

	private static readonly string NewLine = "\n";

	private static AATKitBinding _instance = null;

	private static AndroidJavaObject _plugin;

	public static AATKitBinding Instance => _instance;

	public static bool ScriptLogEnabled
	{
		private get;
		set;
	}

	private static void Log(string message)
	{
		if (ScriptLogEnabled)
		{
			UnityEngine.Debug.Log(LogPrefix + message);
		}
	}

	private void Awake()
	{
		ScriptLogEnabled = true;
		Log("Awake");
		if (_instance != null && _instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		_instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		if (_plugin == null && Application.platform == RuntimePlatform.Android)
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.intentsoftware.addapptr.AATKitActivity"))
			{
				_plugin = androidJavaClass.CallStatic<AndroidJavaObject>("instance", Array.Empty<object>());
			}
		}
	}

	private void OnApplicationPause(bool pause)
	{
		Log("OnApplicationPause pause: " + pause.ToString());
		if (Application.platform == RuntimePlatform.Android && _plugin != null)
		{
			if (pause)
			{
				_plugin.Call("aatkitOnPause");
			}
			else
			{
				_plugin.Call("aatkitOnResume");
			}
		}
	}

	private void OnApplicationQuit()
	{
		Log("OnApplicationQuit");
		if (Application.platform == RuntimePlatform.Android && _plugin != null)
		{
			_plugin.Call("aatkitOnDestroy");
		}
	}

	public static void Init(string objectName, AATKitConfiguration configuration)
	{
		string text = JsonUtility.ToJson(configuration);
		Log("Init objectName: " + objectName + " configuration: " + text);
		if (configuration.ConsentString != string.Empty && configuration.DetailedConsent != AATKitConfiguration.DetailedConsentTypes.ConsentString)
		{
			UnityEngine.Debug.LogWarning("ConsentString configuration will be ignored because DetailedConsent is not set as ConsentString.");
		}
		if (Application.platform == RuntimePlatform.Android && _plugin != null)
		{
			_plugin.Call("aatkitInit", objectName, text);
		}
		if (Application.isEditor && AATKitPlaceholderManager.Instance != null)
		{
			AATKitPlaceholderManager.Instance.Initialize();
		}
	}

	public static void ReconfigureUsingConfiguration(AATKitConfiguration configuration)
	{
		string text = JsonUtility.ToJson(configuration);
		Log("ReconfigureUsingConfiguration configuration: " + text);
		if (configuration.ConsentString != string.Empty && configuration.DetailedConsent != AATKitConfiguration.DetailedConsentTypes.ConsentString)
		{
			UnityEngine.Debug.LogWarning("ConsentString configuration will be ignored because DetailedConsent is not set as ConsentString.");
		}
		if (Application.platform == RuntimePlatform.Android && _plugin != null)
		{
			_plugin.Call("aatkitReconfigureUsingConfiguration", text);
		}
	}

	[Obsolete]
	public static void Init(string objectName)
	{
		Log("Init objectName: " + objectName);
		if (Application.platform == RuntimePlatform.Android && _plugin != null)
		{
			_plugin.Call("aatkitInit", objectName);
		}
	}

	[Obsolete]
	public static void InitWithTestMode(string objectName, int testID)
	{
		Log("InitWithTestMode objectName: " + objectName + " testID: " + testID);
		if (Application.platform == RuntimePlatform.Android && _plugin != null)
		{
			_plugin.Call("aatkitInitWithTestMode", objectName, testID);
		}
	}

	[Obsolete]
	public static void InitWithRulesCaching(string objectName, bool cachingEnabled, string initialRules)
	{
		Log("InitWithRulesCaching objectName: " + objectName + " cachingEnabled: " + cachingEnabled.ToString() + " initialRules: " + initialRules);
		if (Application.platform == RuntimePlatform.Android && _plugin != null)
		{
			_plugin.Call("aatkitInitWithRulesCaching", objectName, cachingEnabled, initialRules);
		}
	}

	public static void SetDebugEnabled()
	{
		Log("SetDebugEnabled");
		if (Application.platform == RuntimePlatform.Android && _plugin != null)
		{
			_plugin.Call("aatkitSetDebugEnabled");
		}
	}

	public static void SetDebugShakeEnabled(bool enabled)
	{
		Log("SetDebugShakeEnabled enabled: " + enabled.ToString());
		if (Application.platform == RuntimePlatform.Android && _plugin != null)
		{
			_plugin.Call("aatkitSetDebugShakeEnabled", enabled);
		}
	}

	public static string GetVersion()
	{
		Log("GetVersion");
		if (Application.platform == RuntimePlatform.Android && _plugin != null)
		{
			return _plugin.Call<string>("aatkitGetVersion", Array.Empty<object>());
		}
		return "unknown";
	}

	public static string GetDebugInfo()
	{
		Log("GetDebugInfo");
		if (Application.platform == RuntimePlatform.Android && _plugin != null)
		{
			return _plugin.Call<string>("aatkitGetDebugInfo", Array.Empty<object>());
		}
		return "unknown";
	}

	public static bool IsNetworkEnabled(int network)
	{
		Log("IsNetworkEnabled network:" + network);
		if (Application.platform == RuntimePlatform.Android && _plugin != null)
		{
			return _plugin.Call<bool>("aatkitIsNetworkEnable", new object[1]
			{
				network
			});
		}
		return false;
	}

	public static void SetNetworkEnabled(int network, bool enabled)
	{
		Log("SetNetworkEnabled network:" + network + " enabled: " + enabled.ToString());
		if (Application.platform == RuntimePlatform.Android && _plugin != null)
		{
			_plugin.Call("aatkitSetNetworkEnabled", network, enabled);
		}
	}

	public static Vector2 CreatePlacement(string placementName, PlacementSize placementSize)
	{
		Log("CreatePlacement placementName:" + placementName + " placementSize: " + placementSize);
		string text = "0x0";
		if (Application.platform == RuntimePlatform.Android && _plugin != null)
		{
			text = _plugin.Call<string>("aatkitCreatePlacement", new object[2]
			{
				placementName,
				(int)placementSize
			});
		}
		if (Application.isEditor && AATKitPlaceholderManager.Instance != null)
		{
			AATKitPlaceholderManager.Instance.CreatePlacement(placementName, placementSize);
		}
		char[] separator = new char[1]
		{
			'x'
		};
		string[] array = text.Split(separator);
		int num;
		int num2;
		try
		{
			num = int.Parse(array[0]);
			num2 = int.Parse(array[1]);
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogException(exception);
			num = 0;
			num2 = 0;
		}
		return new Vector2(num, num2);
	}

	public static void AddPlacementToView(string placementName)
	{
		Log("AddPlacementToView placementName:" + placementName);
		if (Application.platform == RuntimePlatform.Android && _plugin != null)
		{
			_plugin.Call("aatkitAddPlacementToView", placementName);
		}
		if (Application.isEditor && AATKitPlaceholderManager.Instance != null)
		{
			AATKitPlaceholderManager.Instance.AddPlacementToView(placementName);
		}
	}

	public static void RemovePlacementFromView(string placementName)
	{
		Log("RemovePlacementFromView placementName:" + placementName);
		if (Application.platform == RuntimePlatform.Android && _plugin != null)
		{
			_plugin.Call("aatkitRemovePlacementFromView", placementName);
		}
		if (Application.isEditor && AATKitPlaceholderManager.Instance != null)
		{
			AATKitPlaceholderManager.Instance.RemovePlacementFromView(placementName);
		}
	}

	public static void StartPlacementAutoReload(string placementName)
	{
		Log("StartPlacementAutoReload placementName:" + placementName);
		if (Application.platform == RuntimePlatform.Android && _plugin != null)
		{
			_plugin.Call("aatkitStartPlacementAutoReload", placementName);
		}
		if (Application.isEditor && AATKitPlaceholderManager.Instance != null)
		{
			AATKitPlaceholderManager.Instance.ReloadPlacement(placementName);
		}
	}

	public static void StopPlacementAutoReload(string placementName)
	{
		Log("StopPlacementAutoReload placementName:" + placementName);
		if (Application.platform == RuntimePlatform.Android && _plugin != null)
		{
			_plugin.Call("aatkitStopPlacementAutoReload", placementName);
		}
	}

	public static bool ReloadPlacement(string placementName)
	{
		Log("ReloadPlacement placementName:" + placementName);
		if (Application.platform == RuntimePlatform.Android && _plugin != null)
		{
			return _plugin.Call<bool>("aatkitReloadPlacement", new object[1]
			{
				placementName
			});
		}
		if (Application.isEditor && AATKitPlaceholderManager.Instance != null)
		{
			AATKitPlaceholderManager.Instance.ReloadPlacement(placementName);
			return true;
		}
		return false;
	}

	public static bool ReloadPlacementForced(string placementName, bool forced)
	{
		Log("ReloadPlacementForced placementName:" + placementName + " forced: " + forced.ToString());
		if (Application.platform == RuntimePlatform.Android && _plugin != null)
		{
			return _plugin.Call<bool>("aatkitReloadPlacementForced", new object[2]
			{
				placementName,
				forced
			});
		}
		if (Application.isEditor && AATKitPlaceholderManager.Instance != null)
		{
			AATKitPlaceholderManager.Instance.ReloadPlacement(placementName);
			return true;
		}
		return false;
	}

	public static bool HasAdForPlacement(string placementName)
	{
		Log("HasAdForPlacement placementName:" + placementName);
		if (Application.platform == RuntimePlatform.Android && _plugin != null)
		{
			return _plugin.Call<bool>("aatkitHasAdForPlacement", new object[1]
			{
				placementName
			});
		}
		return false;
	}

	public static void StartPlacementAutoReloadWithSeconds(string placementName, int seconds)
	{
		Log("StartPlacementAutoReloadWithSeconds placementName:" + placementName + " seconds: " + seconds);
		if (Application.platform == RuntimePlatform.Android && _plugin != null)
		{
			_plugin.Call("aatkitStartPlacementAutoReloadWithSeconds", placementName, seconds);
		}
		if (Application.isEditor && AATKitPlaceholderManager.Instance != null)
		{
			AATKitPlaceholderManager.Instance.ReloadPlacement(placementName);
		}
	}

	public static void SetMultiSizeAlignment(string placementName, BannerAlignment multiSizeAlignment)
	{
		Log("SetMultiSizeAlignment placementName:" + placementName + " multiSizeAlignment: " + multiSizeAlignment);
		if (Application.platform == RuntimePlatform.Android && _plugin != null)
		{
			_plugin.Call("aatkitSetMultiSizeAlignment", placementName, (int)multiSizeAlignment);
		}
	}

	public static void SetMultiSizeAlignmentWithOffset(string placementName, BannerAlignment multiSizeAlignment, int x, int y)
	{
		Log("SetMultiSizeAlignmentWithOffset placementName:" + placementName + " multiSizeAlignment: " + multiSizeAlignment + " x: " + x + " y: " + y);
		if (Application.platform == RuntimePlatform.Android && _plugin != null)
		{
			_plugin.Call("aatkitSetMultiSizeAlignmentWithOffset", placementName, (int)multiSizeAlignment, x, y);
		}
	}

	public static void SetPlacementAlignment(string placementName, BannerAlignment bannerAlignment)
	{
		Log("SetPlacementAlignment placementName:" + placementName + " bannerAlignment: " + bannerAlignment);
		if (Application.platform == RuntimePlatform.Android && _plugin != null)
		{
			_plugin.Call("aatkitSetPlacementAlignment", placementName, (int)bannerAlignment);
		}
		if (Application.isEditor && AATKitPlaceholderManager.Instance != null)
		{
			AATKitPlaceholderManager.Instance.SetPlacementAlignment(placementName, bannerAlignment);
		}
	}

	public static void SetPlacementAlignmentWithOffset(string placementName, BannerAlignment bannerAlignment, int x, int y)
	{
		Log("SetPlacementAlignmentWithOffset placementName:" + placementName + " bannerAlignment: " + bannerAlignment + " x: " + x + " y:" + y);
		if (Application.platform == RuntimePlatform.Android && _plugin != null)
		{
			_plugin.Call("aatkitSetPlacementAlignmentWithOffset", placementName, (int)bannerAlignment, x, y);
		}
	}

	public static void SetPlacementPosition(string placementName, int posX, int posY)
	{
		Log("SetPlacementPosition placementName:" + placementName + " posX: " + posX + " posY: " + posY);
		if (Application.platform == RuntimePlatform.Android && _plugin != null)
		{
			_plugin.Call("aatkitSetPlacementPosition", placementName, posX, posY);
		}
	}

	public static void SetMultiSizePosition(string placementName, int posX, int posY)
	{
		Log("SetMultiSizePosition placementName:" + placementName + " posX: " + posX + " posY: " + posY);
		if (Application.platform == RuntimePlatform.Android && _plugin != null)
		{
			_plugin.Call("aatkitSetMultiSizePosition", placementName, posX, posY);
		}
	}

	public static bool ShowPlacement(string placementName)
	{
		Log("ShowPlacement placementName:" + placementName);
		if (Application.platform == RuntimePlatform.Android && _plugin != null)
		{
			return _plugin.Call<bool>("aatkitShowPlacement", new object[1]
			{
				placementName
			});
		}
		return false;
	}

	public static bool IsFrequencyCapReachedForPlacement(string placementName)
	{
		Log("IsFrequencyCapReachedForPlacement placementName:" + placementName);
		if (Application.platform == RuntimePlatform.Android && _plugin != null)
		{
			return _plugin.Call<bool>("aatkitIsFrequencyCapReachedForPlacement", new object[1]
			{
				placementName
			});
		}
		return false;
	}

	[Obsolete("Will be removed in a future release: Use preparePromo and showPromo: instead.")]
	public static void EnablePromo()
	{
		Log("EnablePromo");
		if (Application.platform == RuntimePlatform.Android && _plugin != null)
		{
			_plugin.Call("aatkitEnablePromo");
		}
	}

	[Obsolete("Will be removed in a future release: Use preparePromo and showPromo: instead.")]
	public static void DisablePromo()
	{
		Log("DisablePromo");
		if (Application.platform == RuntimePlatform.Android && _plugin != null)
		{
			_plugin.Call("aatkitDisablePromo");
		}
	}

	public static void PreparePromo()
	{
		Log("PreparePromo");
		if (Application.platform == RuntimePlatform.Android && _plugin != null)
		{
			_plugin.Call("aatkitPreparePromo");
		}
	}

	public static bool ShowPromo(bool force = false)
	{
		Log("ShowPromo force: " + force.ToString());
		if (Application.platform == RuntimePlatform.Android && _plugin != null)
		{
			return _plugin.Call<bool>("aatkitShowPromo", new object[1]
			{
				force
			});
		}
		return false;
	}

	public static void AddAdNetworkForKeywordTargeting(AdNetwork network)
	{
		Log("AddAdNetworkForKeywordTargeting network: " + network);
		if (Application.platform == RuntimePlatform.Android && _plugin != null)
		{
			_plugin.Call("aatkitAddAdNetworkForKeywordTargeting", (int)network);
		}
	}

	public static void RemoveAdNetworkForKeywordTargeting(AdNetwork network)
	{
		Log("RemoveAdNetworkForKeywordTargeting network: " + network);
		if (Application.platform == RuntimePlatform.Android && _plugin != null)
		{
			_plugin.Call("aatkitRemoveAdNetworkForKeywordTargeting", (int)network);
		}
	}

	public static void SetTargetingInfo(string placementName, Dictionary<string, List<string>> info)
	{
		Log("SetTargetingInfo placementName: " + placementName + " info: " + CreateCSVForTargetInfo(info));
		if (Application.platform == RuntimePlatform.Android && _plugin != null)
		{
			_plugin.Call("aatkitSetTargetingInfo", placementName, info);
		}
	}

	public static void SetTargetingInfo(Dictionary<string, List<string>> info)
	{
		Log("SetTargetingInfo info: " + CreateCSVForTargetInfo(info));
		if (Application.platform == RuntimePlatform.Android && _plugin != null)
		{
			_plugin.Call("aatkitSetTargetingInfo", CreateAndroidTargetInfo(info));
		}
	}

	private static string CreateCSVForTargetInfo(Dictionary<string, List<string>> info)
	{
		string text = "";
		int num = 0;
		foreach (KeyValuePair<string, List<string>> item in info)
		{
			if (num != 0)
			{
				text += NewLine;
			}
			text += item.Key;
			num++;
			List<string> value = item.Value;
			for (int i = 0; i < value.Count; i++)
			{
				text = text + CSVSeparator + value[i];
			}
		}
		return text;
	}

	private static AndroidJavaObject CreateAndroidTargetInfo(Dictionary<string, List<string>> info)
	{
		AndroidJavaObject androidJavaObject = new AndroidJavaObject("java.util.HashMap");
		foreach (KeyValuePair<string, List<string>> item in info)
		{
			AndroidJavaObject androidJavaObject2 = new AndroidJavaObject("java.util.LinkedList");
			foreach (string item2 in item.Value)
			{
				AndroidJavaObject androidJavaObject3 = new AndroidJavaObject("java.lang.String", item2);
				androidJavaObject2.Call<bool>("add", new object[1]
				{
					androidJavaObject3
				});
			}
			AndroidJavaObject androidJavaObject4 = new AndroidJavaObject("java.lang.String", item.Key);
			IntPtr methodID = AndroidJNIHelper.GetMethodID(androidJavaObject.GetRawClass(), "put", "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");
			AndroidJNI.CallObjectMethod(args: AndroidJNIHelper.CreateJNIArgArray(new object[2]
			{
				androidJavaObject4,
				androidJavaObject2
			}), obj: androidJavaObject.GetRawObject(), methodID: methodID);
		}
		return androidJavaObject;
	}

	public static void SetPlacementContentGravity(string placementName, PlacementContentGravity gravity)
	{
		Log("SetPlacementContentGravity placementName: " + placementName + " gravity: " + gravity);
		if (Application.platform == RuntimePlatform.Android && _plugin != null)
		{
			_plugin.Call("aatkitSetPlacementContentGravity", placementName, (int)gravity);
		}
	}

	public static bool ShowConsentDialogIfNeeded()
	{
		Log("ShowConsentDialogIfNeeded");
		if (Application.platform == RuntimePlatform.Android && _plugin != null)
		{
			return _plugin.Call<bool>("aatkitShowConsentDialogIfNeeded", Array.Empty<object>());
		}
		return false;
	}

	public static void ShowConsentDialog()
	{
		Log("ShowConsentDialog");
		if (Application.platform == RuntimePlatform.Android && _plugin != null)
		{
			_plugin.Call("aatkitShowConsentDialog");
		}
	}

	public static float GetScale()
	{
		Log("GetScale");
		return 0f;
	}

	public static float GetNativeScale()
	{
		Log("GetNativeScale");
		return 0f;
	}
}
