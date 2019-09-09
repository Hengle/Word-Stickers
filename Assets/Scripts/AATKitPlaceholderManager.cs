using System.Collections.Generic;
using UnityEngine;

public class AATKitPlaceholderManager : MonoBehaviour
{
	[SerializeField]
	private AATKitBanner bannerPrefab;

	private Dictionary<string, AATKitBanner> placements;

	private bool initialized;

	public static AATKitPlaceholderManager Instance
	{
		get;
		private set;
	}

	private void Awake()
	{
		if (!Application.isEditor || Instance != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		Instance = this;
		Object.DontDestroyOnLoad(base.gameObject);
	}

	public void Initialize()
	{
		Log("Initialize");
		if (!initialized)
		{
			placements = new Dictionary<string, AATKitBanner>();
			initialized = true;
		}
		else
		{
			Log("Ignoring Initialize method call. AAtKit already initialized.");
		}
	}

	public void CreatePlacement(string placementName, AATKitBinding.PlacementSize placementSize)
	{
		Log("CreatePlacement placementName: " + placementName + " placementSize: " + placementSize);
		if (IsInitialized())
		{
			if (placements.ContainsKey(placementName))
			{
				Log("Cannot create placement. " + placementName + " already exists.");
			}
			if (placementSize == AATKitBinding.PlacementSize.Fullscreen || placementSize == AATKitBinding.PlacementSize.MultiSizeBanner || placementSize == AATKitBinding.PlacementSize.Rewarded)
			{
				Log("Ignoring creating placement. Placement size " + placementSize + " is not supported in the Unity Editor.");
				return;
			}
			AATKitBanner aATKitBanner = UnityEngine.Object.Instantiate(bannerPrefab, base.transform);
			aATKitBanner.gameObject.name = placementName;
			aATKitBanner.placementSize = placementSize;
			placements.Add(placementName, aATKitBanner);
		}
	}

	public void ReloadPlacement(string placementName)
	{
		Log("ReloadPlacement placementName: " + placementName);
		if (IsInitialized() && PlacementExists(placementName))
		{
			placements[placementName].Show();
		}
	}

	public void StartPlacementAutoReload(string placementName)
	{
		Log("StartPlacementAutoReload placementName: " + placementName);
		if (IsInitialized())
		{
			ReloadPlacement(placementName);
		}
	}

	public void AddPlacementToView(string placementName)
	{
		Log("AddPlacementToView placementName: " + placementName);
		if (IsInitialized() && PlacementExists(placementName))
		{
			placements[placementName].Show();
		}
	}

	public void RemovePlacementFromView(string placementName)
	{
		Log("RemovePlacementFromView placementName: " + placementName);
		if (IsInitialized() && PlacementExists(placementName))
		{
			placements[placementName].Hide();
		}
	}

	public void SetPlacementAlignment(string placementName, AATKitBinding.BannerAlignment bannerAlignment)
	{
		Log("SetPlacementAlignment placementName: " + placementName + " bannerAlignment: " + bannerAlignment);
		if (IsInitialized() && PlacementExists(placementName))
		{
			AATKitBanner aATKitBanner = placements[placementName];
			switch (bannerAlignment)
			{
			case AATKitBinding.BannerAlignment.TopLeft:
				aATKitBanner.SetPositionOnTheScreen(new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(0f, 1f));
				break;
			case AATKitBinding.BannerAlignment.TopCenter:
				aATKitBanner.SetPositionOnTheScreen(new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0.5f, 1f));
				break;
			case AATKitBinding.BannerAlignment.TopRight:
				aATKitBanner.SetPositionOnTheScreen(new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(1f, 1f));
				break;
			case AATKitBinding.BannerAlignment.BottomLeft:
				aATKitBanner.SetPositionOnTheScreen(new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(0f, 0f));
				break;
			case AATKitBinding.BannerAlignment.BottomCenter:
				aATKitBanner.SetPositionOnTheScreen(new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f));
				break;
			case AATKitBinding.BannerAlignment.BottomRight:
				aATKitBanner.SetPositionOnTheScreen(new Vector2(1f, 0f), new Vector2(1f, 0f), new Vector2(1f, 0f));
				break;
			}
		}
	}

	private bool PlacementExists(string placementName)
	{
		if (placements.ContainsKey(placementName))
		{
			return true;
		}
		Log(placementName + " does not exists");
		return false;
	}

	private bool IsInitialized()
	{
		if (initialized)
		{
			return true;
		}
		Log("Error: AAtKit not initialized.");
		return false;
	}

	private static void Log(string message)
	{
		UnityEngine.Debug.Log("[" + typeof(AATKitPlaceholderManager) + "] " + message);
	}
}
