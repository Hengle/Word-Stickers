using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class ShopPopUp : CommonPopUp<ShopPopUp>
{
	private const float SCROLL_EXTRA_SIZE_IPHONE_5 = 400f;

	private const float SCROLL_EXTRA_SIZE_IPHONE_X = 450f;

	public RectTransform background;

	public ScrollRect scrollRect;

	public GameObject content;

	public Transform title;

	public CommonButton exitButton;

	public IapItem iapPrefabCoins;

	public IapItemHints iapPrefabHints;

	public IapItem iapPrefabPacks;

	public IapItem iapPrefabRemoveAds;

	public IapTitle iapTitlePrefab;

	public IapRevardedVideo iapRevardedVideo;

	private const float TOP_OFFSET_Y = 60f;

	private List<RectTransform> itemList;

	public new void Start()
	{
		base.Start();
		RectTransform component = scrollRect.transform.GetComponent<RectTransform>();
		float num = (ELSingleton<ELCanvas>.Instance.CanvasRatio == ELCanvas.Ratio.iPhone5) ? 400f : ((ELSingleton<ELCanvas>.Instance.CanvasRatio == ELCanvas.Ratio.iPhoneX) ? 450f : 0f);
		background.sizeDelta = new Vector2(background.sizeDelta.x, background.sizeDelta.y + num);
		component.sizeDelta = new Vector2(component.sizeDelta.x, component.sizeDelta.y + num);
		title.localPosition = new Vector3(title.localPosition.x, title.localPosition.y + num / 2f, 0f);
		exitButton.transform.localPosition = new Vector3(exitButton.transform.localPosition.x, exitButton.transform.localPosition.y + num / 2f, 0f);
	}

	private new void Update()
	{
		base.Update();
		if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
		{
			ExitButton();
		}
		IapItem[] componentsInChildren = base.gameObject.GetComponentsInChildren<IapItem>(includeInactive: true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].CheckVisible();
		}
		IapRevardedVideo[] componentsInChildren2 = base.gameObject.GetComponentsInChildren<IapRevardedVideo>(includeInactive: true);
		for (int i = 0; i < componentsInChildren2.Length; i++)
		{
			componentsInChildren2[i].CheckVisible();
		}
		IapTitle[] componentsInChildren3 = base.gameObject.GetComponentsInChildren<IapTitle>(includeInactive: true);
		for (int i = 0; i < componentsInChildren3.Length; i++)
		{
			componentsInChildren3[i].CheckVisible();
		}
	}

	private float UpdateLayout()
	{
		float num = 60f;
		foreach (RectTransform item in itemList)
		{
			float num2 = item.sizeDelta.y + 5f;
			Vector2 sizeDeltum = item.sizeDelta;
			item.transform.localPosition = new Vector2(0f, 0f - num - num2 / 2f);
			num += num2;
		}
		num += 60f;
		foreach (RectTransform item2 in itemList)
		{
			item2.transform.localPosition = new Vector2(item2.transform.localPosition.x, item2.transform.localPosition.y + num / 2f);
		}
		return num;
	}

	private void LateUpdate()
	{
		UpdateLayout();
	}

	public new void ShowPopUp(float aDelay = 0f)
	{
		if (ELSingleton<IapManager>.Instance.IsInitialized())
		{
			base.ShowPopUp(aDelay);
			exitButton.Enable();
			foreach (Transform item in content.transform)
			{
				item.gameObject.SetActive(value: false);
				UnityEngine.Object.Destroy(item.gameObject);
			}
			itemList = new List<RectTransform>();
			int num = 0;
			IapTitle iapTitle = Object.Instantiate(iapTitlePrefab, content.transform);
			iapTitle.Title = "Free Coins!";
			itemList.Add(iapTitle.GetComponent<RectTransform>());
			IapRevardedVideo iapRevardedVideo = Object.Instantiate(this.iapRevardedVideo, content.transform);
			iapRevardedVideo.Prepare();
			itemList.Add(iapRevardedVideo.GetComponent<RectTransform>());
			if (!ELSingleton<IapManager>.Instance.initialOfferConsummed)
			{
				foreach (XmlSettings.IapConfig item2 in ELSingleton<XmlSettings>.Instance.iapConfig)
				{
                    if (item2.set == 1)
                    {
                        if (item2.type == ProductType.Consumable && item2.coins > 0 && (item2.expose > 0 || item2.goodStart > 0 || item2.startFinish > 0 || item2.tip > 0))
                        {
                            IapTitle iapTitle2 = Object.Instantiate(iapTitlePrefab, content.transform);
                            iapTitle2.Title = "One Time Offer!";
                            itemList.Add(iapTitle2.GetComponent<RectTransform>());
                            IapItem iapItem = Object.Instantiate(iapPrefabPacks, content.transform);
                            iapItem.Prepare(item2);
                            itemList.Add(iapItem.GetComponent<RectTransform>());
                        }
                        num++;
                    }
                }
			}
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			foreach (XmlSettings.IapConfig item3 in ELSingleton<XmlSettings>.Instance.iapConfig)
			{
				if (item3.set == 0)
				{
                    if (item3.type == ProductType.Consumable && item3.coins > 0 && item3.expose == 0 && item3.goodStart == 0 && item3.startFinish == 0 && item3.tip == 0)
                    {
                        if (!flag)
                        {
                            IapTitle iapTitle3 = Object.Instantiate(iapTitlePrefab, content.transform);
                            iapTitle3.Title = "Coins";
                            itemList.Add(iapTitle3.GetComponent<RectTransform>());
                            flag = true;
                        }
                        IapItem iapItem2 = Object.Instantiate(iapPrefabCoins, content.transform);
                        iapItem2.Prepare(item3);
                        itemList.Add(iapItem2.GetComponent<RectTransform>());
                    }
                    if (item3.type == ProductType.Consumable && item3.coins == 0 && (item3.expose == 0 || item3.goodStart == 0 || item3.startFinish == 0 || item3.tip == 0))
                    {
                        if (!flag2)
                        {
                            IapTitle iapTitle4 = Object.Instantiate(iapTitlePrefab, content.transform);
                            iapTitle4.Title = "Hints";
                            itemList.Add(iapTitle4.GetComponent<RectTransform>());
                            flag2 = true;
                        }
                        IapItemHints iapItemHints = Object.Instantiate(iapPrefabHints, content.transform);
                        iapItemHints.Prepare(item3);
                        itemList.Add(iapItemHints.GetComponent<RectTransform>());
                    }
                    if (item3.type == ProductType.Consumable && item3.coins > 0 && (item3.expose > 0 || item3.goodStart > 0 || item3.startFinish > 0 || item3.tip > 0))
                    {
                        if (!flag3)
                        {
                            IapTitle iapTitle5 = Object.Instantiate(iapTitlePrefab, content.transform);
                            iapTitle5.Title = "Packs";
                            itemList.Add(iapTitle5.GetComponent<RectTransform>());
                            flag3 = true;
                        }
                        IapItem iapItem3 = Object.Instantiate(iapPrefabPacks, content.transform);
                        iapItem3.Prepare(item3);
                        itemList.Add(iapItem3.GetComponent<RectTransform>());
                    }
                    if (item3.type == ProductType.NonConsumable && item3.noads)
                    {
                        IapTitle iapTitle6 = Object.Instantiate(iapTitlePrefab, content.transform);
                        iapTitle6.Title = "Remove Ads";
                        itemList.Add(iapTitle6.GetComponent<RectTransform>());
                        IapItem iapItem4 = Object.Instantiate(iapPrefabRemoveAds, content.transform);
                        iapItem4.Prepare(item3);
                        itemList.Add(iapItem4.GetComponent<RectTransform>());
                    }
                    num++;
                }
			}
			float y = UpdateLayout();
			RectTransform component = content.GetComponent<RectTransform>();
			component.sizeDelta = new Vector2(component.sizeDelta.x, y);
			scrollRect.verticalNormalizedPosition = 1f;
			StartCoroutine(SetScrollValue());
			float num2 = 0f;
			Component[] componentsInChildren = base.gameObject.GetComponentsInChildren<CommonButton>();
			componentsInChildren = componentsInChildren;
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				((CommonButton)componentsInChildren[i]).Show(num2);
				num2 += 0.1f;
			}
		}
		else
		{
			ELSingleton<MessagePopUp>.Instance.ShowPopUp("Shop", "Shop is not ready. Try again later.");
		}
	}

	private IEnumerator SetScrollValue()
	{
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		scrollRect.verticalNormalizedPosition = 1f;
		yield return null;
	}

	public void ExitButton()
	{
		exitButton.Disable();
		HidePopUp();
	}
}
