using System;
using System.Collections.Generic;
using UnityEngine;

public class AATKitBanner : MonoBehaviour
{
	private static readonly int IPadScreenWidth = 1536;

	private static readonly Dictionary<int, float> AndroidScaleByScreenWidth = new Dictionary<int, float>();

	private static readonly Dictionary<int, float> IphoneScaleByScreenWidth = new Dictionary<int, float>();

	private static readonly Dictionary<AATKitBinding.PlacementSize, Vector2> BannerSizeByPlacementSize = new Dictionary<AATKitBinding.PlacementSize, Vector2>();

	[SerializeField]
	public AATKitBinding.PlacementSize placementSize;

	[SerializeField]
	private RectTransform content;

	private RectTransform rect;

	private void Awake()
	{
		rect = GetComponent<RectTransform>();
		InitializeBannerSizeByPlacementSize();
		InitializeAndroidScaleByScreenWidth();
		InitializeIphoneScaleByScreenWidth();
	}

	private static void InitializeBannerSizeByPlacementSize()
	{
		BannerSizeByPlacementSize.Add(AATKitBinding.PlacementSize.Banner320x53, new Vector2(320f, 53f));
		BannerSizeByPlacementSize.Add(AATKitBinding.PlacementSize.Banner300x250, new Vector2(300f, 250f));
		BannerSizeByPlacementSize.Add(AATKitBinding.PlacementSize.Banner768x90, new Vector2(768f, 90f));
		BannerSizeByPlacementSize.Add(AATKitBinding.PlacementSize.BannerAuto, new Vector2(0f, 0f));
		BannerSizeByPlacementSize.Add(AATKitBinding.PlacementSize.Fullscreen, new Vector2(0f, 0f));
		BannerSizeByPlacementSize.Add(AATKitBinding.PlacementSize.MultiSizeBanner, new Vector2(0f, 0f));
		BannerSizeByPlacementSize.Add(AATKitBinding.PlacementSize.Rewarded, new Vector2(0f, 0f));
	}

	private static void InitializeAndroidScaleByScreenWidth()
	{
		AndroidScaleByScreenWidth.Add(1440, 4f);
		AndroidScaleByScreenWidth.Add(960, 3f);
		AndroidScaleByScreenWidth.Add(720, 2f);
		AndroidScaleByScreenWidth.Add(480, 1.5f);
		AndroidScaleByScreenWidth.Add(320, 1f);
		AndroidScaleByScreenWidth.Add(200, 0.75f);
	}

	private static void InitializeIphoneScaleByScreenWidth()
	{
		IphoneScaleByScreenWidth.Add(1536, 2f);
		IphoneScaleByScreenWidth.Add(1125, 3f);
		IphoneScaleByScreenWidth.Add(1080, 2.608f);
		IphoneScaleByScreenWidth.Add(640, 2f);
	}

	private void Update()
	{
		if (Application.isEditor)
		{
			UpdateBannerSize(AndroidScaleByScreenWidth);
		}
	}

	private void UpdateBannerSize(Dictionary<int, float> scaleByScreenWidth)
	{
		int num = Math.Min(Screen.width, Screen.height);
		foreach (KeyValuePair<int, float> item in scaleByScreenWidth)
		{
			if (num >= item.Key)
			{
				Vector2 a = CalculateBannerSize();
				float value = item.Value;
				rect.sizeDelta = a * value;
				break;
			}
		}
	}

	private Vector2 CalculateBannerSize()
	{
		Vector2 result = BannerSizeByPlacementSize[placementSize];
		if (placementSize == AATKitBinding.PlacementSize.BannerAuto)
		{
			result = BannerSizeByPlacementSize[AATKitBinding.PlacementSize.Banner320x53];
		}
		return result;
	}

	public void Show()
	{
		content.gameObject.SetActive(value: true);
	}

	public void Hide()
	{
		content.gameObject.SetActive(value: false);
	}

	public void SetPositionOnTheScreen(Vector2 pivot, Vector2 anchorMin, Vector2 anchorMax)
	{
		rect.pivot = pivot;
		rect.anchorMin = anchorMin;
		rect.anchorMax = anchorMax;
		rect.anchoredPosition = Vector2.zero;
	}
}
