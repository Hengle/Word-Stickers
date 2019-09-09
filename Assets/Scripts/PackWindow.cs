using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PackWindow : ELWindow<PackWindow>
{
	public enum AnimationState
	{
		ANIMATION_STATE_NONE,
		ANIMATION_STATE_PACK_WAIT,
		ANIMATION_STATE_PACK_WELL_DONE,
		ANIMATION_STATE_PACK_COINS,
		ANIMATION_STATE_PACK_OPEN_NEXT,
		ANIMATION_STATE_PACK_DONE
	}

	private const float TOP_OFFSET_Y = 150f;

	[HideInInspector]
	public bool actionTurnOff;

	public GameObject content;

	public ScrollRect scrollRect;

	public PackItem packItemPrefab;

	private List<PackItem> packItemList;

	public ShopButton shopButton;

	public CommonButton backButton;

	public RectTransform menuBar;

	private AnimationState animationState;

	public Animator wellDoneAnimation;

	public CommonCoinPod coinPod;

	public Transform coinTarget;

	private PackItem packToUnlock;

	private PackItem packToComplete;

	private bool lastPackCompleted;

	public AudioClip wellDoneSound;

	public new void Start()
	{
		base.Start();
		if (ELDevice.HasNotch())
		{
			backButton.transform.localPosition = new Vector3(backButton.transform.localPosition.x, backButton.transform.localPosition.y - ELDevice.notchOffsetY, 0f);
			shopButton.transform.localPosition = new Vector3(shopButton.transform.localPosition.x, shopButton.transform.localPosition.y - ELDevice.notchOffsetY, 0f);
			menuBar.transform.localPosition = new Vector3(menuBar.transform.localPosition.x, menuBar.transform.localPosition.y - ELDevice.notchOffsetY, 0f);
			menuBar.sizeDelta += new Vector2(0f, ELDevice.notchOffsetY * 2f);
		}
	}

	private void Update()
	{
		if (!ELSingleton<PopUpManager>.Instance.IsActiveAndEnabled() && UnityEngine.Input.GetKeyDown(KeyCode.Escape))
		{
			BackButton();
		}
		PackItem[] componentsInChildren = base.gameObject.GetComponentsInChildren<PackItem>(includeInactive: true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].CheckVisible();
		}
	}

	private void LateUpdate()
	{
		UpdateLayout();
	}

	private float UpdateLayout()
	{
		float num = 150f;
		if (ELDevice.HasNotch())
		{
			num += ELDevice.notchOffsetY;
		}
		foreach (PackItem packItem in packItemList)
		{
			float num2 = packItem.backRect.sizeDelta.y + 30f;
			packItem.transform.localPosition = new Vector2(0f, 0f - num - num2 / 2f);
			packItem.normalizedScrollLocation = num;
			num += num2;
		}
		num += 150f;
		foreach (PackItem packItem2 in packItemList)
		{
			packItem2.transform.localPosition = new Vector2(packItem2.transform.localPosition.x, packItem2.transform.localPosition.y + num / 2f);
			packItem2.normalizedScrollLocation = 1f - packItem2.normalizedScrollLocation / (num - 150f);
		}
		return num;
	}

	public void UpdateAnimation(AnimationState state)
	{
		switch (state)
		{
		case AnimationState.ANIMATION_STATE_PACK_WAIT:
			actionTurnOff = true;
			scrollRect.enabled = false;
			break;
		case AnimationState.ANIMATION_STATE_PACK_WELL_DONE:
			wellDoneAnimation.SetTrigger("Show");
			Invoke("AnimationCoinsInvoke", 2f);
			break;
		case AnimationState.ANIMATION_STATE_PACK_COINS:
			coinPod.ReleaseCoinsRadial(ELSingleton<XmlSettings>.Instance.coinsConfig.pack, base.transform.position, coinTarget.position, 1f, 1f, 1f, 0f, aIsFullUpdate: false, aIsWithParticles: false);
			Invoke("AnimationOpenInvoke", 2f);
			break;
		case AnimationState.ANIMATION_STATE_PACK_OPEN_NEXT:
			if ((bool)packToComplete)
			{
				packToComplete.UpdateLook();
			}
			if ((bool)packToUnlock)
			{
				packToUnlock.OpenAfterComplete();
				Invoke("AnimationDoneInvoke", 2f);
			}
			else
			{
				Invoke("AnimationDoneInvoke", 0f);
			}
			break;
		case AnimationState.ANIMATION_STATE_PACK_DONE:
			actionTurnOff = false;
			scrollRect.enabled = true;
			PopUpsActionInvoke();
			break;
		}
		animationState = state;
	}

	public void AnimationWellDoneInvoke()
	{
		UpdateAnimation(AnimationState.ANIMATION_STATE_PACK_WELL_DONE);
	}

	public void AnimationWellDoneInvokeSfx()
	{
		ELSingleton<AudioManager>.Instance.PlaySfx(wellDoneSound);
	}

	public void AnimationCoinsInvoke()
	{
		UpdateAnimation(AnimationState.ANIMATION_STATE_PACK_COINS);
	}

	public void AnimationOpenInvoke()
	{
		UpdateAnimation(AnimationState.ANIMATION_STATE_PACK_OPEN_NEXT);
	}

	public void AnimationDoneInvoke()
	{
		UpdateAnimation(AnimationState.ANIMATION_STATE_PACK_DONE);
	}

	public void PopUpsActionInvoke()
	{
		ELSingleton<RateUsManager>.Instance.Trigger();
		ELSingleton<ShareManager>.Instance.Trigger();
		if (lastPackCompleted)
		{
			ELSingleton<CongratulationsPopUp>.Instance.ShowPopUp();
		}
		else if (ELSingleton<RateUsManager>.Instance.ShouldShow())
		{
			ELSingleton<RateUsManager>.Instance.Show();
			ELSingleton<RateUsPopUp>.Instance.ShowPopUp();
		}
		else if (ELSingleton<ShareManager>.Instance.ShouldShow())
		{
			ELSingleton<ShareManager>.Instance.Show();
			ELSingleton<SharePopUp>.Instance.ShowPopUp();
		}
		else if (ELSingleton<FacebookManager>.Instance.ShouldShow())
		{
			ELSingleton<FacebookPopUp>.Instance.ShowPopUp();
		}
	}

	public void ShowWindow(LevelInfo aLevelInfo, AnimationState state = AnimationState.ANIMATION_STATE_NONE)
	{
		CancelInvoke();
		ShowWindow();
		UpdateAnimation(state);
		shopButton.Reset();
		foreach (Transform item in content.transform)
		{
			item.gameObject.SetActive(value: false);
			UnityEngine.Object.Destroy(item.gameObject);
		}
		packItemList = new List<PackItem>();
		int num = 0;
		float num2 = 0f;
		for (int i = 0; i < ELSingleton<LevelsSettings>.Instance.levelSet.worlds.Count; i++)
		{
			for (int j = 0; j < ELSingleton<LevelsSettings>.Instance.levelSet.worlds[i].packs.Count; j++)
			{
				aLevelInfo.currentWorld = i;
				aLevelInfo.currentPack = j;
				PackItem packItem = Object.Instantiate(packItemPrefab, content.transform);
				num2 = Mathf.Max(packItem.Prepare(aLevelInfo, num, state), num2);
				if (packItem.IsUnlocked || ELSingleton<ApplicationSettings>.Instance.DeploymentSettings.isCheatsEnabled)
				{
					packItem.OnFocus.AddListener(delegate
					{
						Focus(aLevelInfo);
					});
				}
				packItemList.Add(packItem);
				num++;
			}
		}
		float y = UpdateLayout() + 850f;
		RectTransform component = content.GetComponent<RectTransform>();
		component.sizeDelta = new Vector2(component.sizeDelta.x, y);
		scrollRect.verticalNormalizedPosition = 0f;
		lastPackCompleted = true;
		for (int k = 0; k < packItemList.Count; k++)
		{
			PackItem packItem2 = packItemList[k];
			if (packItem2.IsCurrent)
			{
				if (k > 0)
				{
					packToComplete = packItemList[k - 1];
				}
				packToUnlock = packItem2;
				scrollRect.verticalNormalizedPosition = packItem2.normalizedScrollLocation;
				if (state == AnimationState.ANIMATION_STATE_PACK_WAIT)
				{
					scrollRect.verticalNormalizedPosition = packToComplete.normalizedScrollLocation;
				}
				lastPackCompleted = false;
			}
		}
		if (state == AnimationState.ANIMATION_STATE_PACK_WAIT)
		{
			shopButton.UpdateCoins(-ELSingleton<XmlSettings>.Instance.coinsConfig.pack);
			Invoke("AnimationWellDoneInvoke", num2 + 0.5f);
			Invoke("AnimationWellDoneInvokeSfx", num2 + 0.4f);
		}
		if (state == AnimationState.ANIMATION_STATE_NONE && lastPackCompleted)
		{
			packItemList[packItemList.Count - 1].OpenAfterComplete();
		}
		float num3 = 0f;
		Component[] componentsInChildren = base.gameObject.GetComponentsInChildren<CommonButton>();
		componentsInChildren = componentsInChildren;
		for (int l = 0; l < componentsInChildren.Length; l++)
		{
			((CommonButton)componentsInChildren[l]).Show(num3);
			num3 += 0.1f;
		}
		Screen.sleepTimeout = -2;
	}

	public void Focus(LevelInfo aLevelInfo)
	{
		foreach (PackItem packItem in packItemList)
		{
			if (packItem.levelInfo.Compare(aLevelInfo) != 0)
			{
				packItem.CloseItem();
			}
		}
	}

	public void BackButton()
	{
		if (!actionTurnOff)
		{
			ELSingleton<ApplicationManager>.Instance.ShowMenuAfterPack();
		}
	}

	public void ShopButton()
	{
		if (!actionTurnOff)
		{
			ELSingleton<ShopPopUp>.Instance.ShowPopUp();
		}
	}
}
