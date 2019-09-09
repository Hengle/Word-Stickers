using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FBRankingPopUpPopUp : CommonPopUp<FBRankingPopUpPopUp>
{
	private const float SCROLL_EXTRA_SIZE_IPHONE_5 = 400f;

	private const float SCROLL_EXTRA_SIZE_IPHONE_X = 450f;

	public GameObject loading;

	public Text messagePrefab;

	public CommonButton inviteButton;

	public RankingPlayer playerPrefabFB;

	public RankingPlayer playerPrefabNormal;

	public RectTransform background;

	public ScrollRect scrollRect;

	public GameObject content;

	public Transform title;

	public CommonButton exitButton;

	public CommonButton friendButton;

	public CommonButton globalButton;

	private List<RectTransform> itemList;

	private const float TOP_OFFSET_Y = 20f;

	private bool setScrollPosition;

	private float scrollPosition = 1f;

	public new void Start()
	{
		base.Start();
		RectTransform component = scrollRect.transform.GetComponent<RectTransform>();
		float num = (ELSingleton<ELCanvas>.Instance.CanvasRatio == ELCanvas.Ratio.iPhone5) ? 400f : ((ELSingleton<ELCanvas>.Instance.CanvasRatio == ELCanvas.Ratio.iPhoneX) ? 450f : 0f);
		background.sizeDelta = new Vector2(background.sizeDelta.x, background.sizeDelta.y + num);
		component.sizeDelta = new Vector2(component.sizeDelta.x, component.sizeDelta.y + num);
		title.localPosition = new Vector3(title.localPosition.x, title.localPosition.y + num / 2f, 0f);
		exitButton.transform.localPosition = new Vector3(exitButton.transform.localPosition.x, exitButton.transform.localPosition.y + num / 2f, 0f);
		friendButton.transform.localPosition = new Vector3(friendButton.transform.localPosition.x, friendButton.transform.localPosition.y - num / 2f, 0f);
		globalButton.transform.localPosition = new Vector3(globalButton.transform.localPosition.x, globalButton.transform.localPosition.y - num / 2f, 0f);
	}

	private new void Update()
	{
		base.Update();
		if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
		{
			ExitButton();
		}
		RankingPlayer[] componentsInChildren = base.gameObject.GetComponentsInChildren<RankingPlayer>(includeInactive: true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].CheckVisible();
		}
	}

	public void FriendsButton()
	{
		foreach (Transform item in content.transform)
		{
			item.gameObject.SetActive(value: false);
			UnityEngine.Object.Destroy(item.gameObject);
		}
		itemList = new List<RectTransform>();
		loading.SetActive(value: true);
		ELSingleton<FacebookManager>.Instance.UpdateRanking(FacebookManager.FBRankingType.FRIENDS, this, "RankingCallback");
	}

	public void GlobalButton()
	{
		foreach (Transform item in content.transform)
		{
			item.gameObject.SetActive(value: false);
			UnityEngine.Object.Destroy(item.gameObject);
		}
		itemList = new List<RectTransform>();
		loading.SetActive(value: true);
		ELSingleton<FacebookManager>.Instance.UpdateRanking(FacebookManager.FBRankingType.GLOBAL, this, "RankingCallback");
	}

	public new void ShowPopUp(float aDelay = 0f)
	{
		base.ShowPopUp(aDelay);
		exitButton.Enable();
		foreach (Transform item in content.transform)
		{
			item.gameObject.SetActive(value: false);
			UnityEngine.Object.Destroy(item.gameObject);
		}
		itemList = new List<RectTransform>();
		loading.SetActive(value: true);
		ELSingleton<FacebookManager>.Instance.UpdateRanking(FacebookManager.FBRankingType.FRIENDS, this, "RankingCallback");
	}

	public void RankingCallback()
	{
		float verticalNormalizedPosition = 1f;
		if (ELSingleton<FacebookManager>.Instance.currentRanking.state == FacebookManager.FBRankingState.DONE)
		{
			loading.SetActive(value: false);
			if (ELSingleton<FacebookManager>.Instance.currentRanking.players.Count == 0)
			{
				Text text = Object.Instantiate(messagePrefab, content.transform);
				text.text = "Nobody from your friends play Word Stickers!?\nInvite your friend to compete in friend leaderboard";
				itemList.Add(text.GetComponent<RectTransform>());
			}
			int num = 0;
			foreach (Player player in ELSingleton<FacebookManager>.Instance.currentRanking.players)
			{
				if (num == 10 && ELSingleton<FacebookManager>.Instance.currentRanking.type == FacebookManager.FBRankingType.GLOBAL)
				{
					Text text2 = Object.Instantiate(messagePrefab, content.transform);
					text2.text = "...";
					RectTransform component = text2.GetComponent<RectTransform>();
					component.sizeDelta = new Vector2(component.sizeDelta.x, 30f);
					itemList.Add(component);
				}
				if (ELSingleton<FacebookManager>.Instance.currentRanking.type == FacebookManager.FBRankingType.FRIENDS)
				{
					player.rank = num + 1;
				}
				if (player.id.Equals(ELSingleton<FacebookManager>.Instance.player.id))
				{
					verticalNormalizedPosition = 1f - 1f * (float)num / (float)(ELSingleton<FacebookManager>.Instance.currentRanking.players.Count - 1);
				}
				RankingPlayer rankingPlayer = Object.Instantiate((ELSingleton<FacebookManager>.Instance.currentRanking.type == FacebookManager.FBRankingType.FRIENDS) ? playerPrefabFB : playerPrefabNormal, content.transform);
				rankingPlayer.Prepare(player, ELSingleton<FacebookManager>.Instance.GetAvatarSprite(player.fbId));
				itemList.Add(rankingPlayer.GetComponent<RectTransform>());
				num++;
			}
			CommonButton commonButton = Object.Instantiate(inviteButton, content.transform);
			commonButton.Show();
			commonButton.OnClick.AddListener(InviteButton);
			itemList.Add(commonButton.GetComponent<RectTransform>());
		}
		else if (ELSingleton<FacebookManager>.Instance.currentRanking.state == FacebookManager.FBRankingState.ERROR)
		{
			loading.SetActive(value: false);
			Text text3 = Object.Instantiate(messagePrefab, content.transform);
			itemList.Add(text3.GetComponent<RectTransform>());
		}
		else
		{
			loading.SetActive(value: true);
		}
		float y = UpdateLayout();
		RectTransform component2 = content.GetComponent<RectTransform>();
		component2.sizeDelta = new Vector2(component2.sizeDelta.x, y);
		scrollRect.verticalNormalizedPosition = verticalNormalizedPosition;
		scrollPosition = verticalNormalizedPosition;
		setScrollPosition = true;
	}

	public void InviteButton()
	{
		ELSingleton<SharePopUp>.Instance.ShareAction();
	}

	public void ExitButton()
	{
		exitButton.Disable();
		HidePopUp();
	}

	private float UpdateLayout()
	{
		float num = 20f;
		foreach (RectTransform item in itemList)
		{
			float num2 = item.sizeDelta.y + 5f;
			Vector2 sizeDeltum = item.sizeDelta;
			item.transform.localPosition = new Vector2(0f, 0f - num - num2 / 2f);
			num += num2;
		}
		num += 20f;
		foreach (RectTransform item2 in itemList)
		{
			item2.transform.localPosition = new Vector2(item2.transform.localPosition.x, item2.transform.localPosition.y + num / 2f);
		}
		return num;
	}

	private void LateUpdate()
	{
		UpdateLayout();
		if (setScrollPosition)
		{
			scrollRect.verticalNormalizedPosition = scrollPosition;
			setScrollPosition = false;
		}
	}
}
