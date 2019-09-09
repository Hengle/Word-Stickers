using UnityEngine;
using UnityEngine.UI;

public class IapRevardedVideo : MonoBehaviour
{
	public Text coinsText;

	public Image playImage;

	public Text timeText;

	private bool isVisible = true;

	public void Prepare()
	{
		coinsText.text = string.Concat(ELSingleton<XmlSettings>.Instance.rewardedVideoAdConfig.coins);
	}

	private void Update()
	{
		if (!ELSingleton<AdsManager>.Instance.IsRewardedVideoAd)
		{
			playImage.color = new Color(1f, 1f, 1f, 0.25f);
			timeText.text = "Try Again Later";
		}
		else if (ELSingleton<IapManager>.Instance.ShowRewardedVideo(aIsExecute: false))
		{
			playImage.color = new Color(1f, 1f, 1f, 1f);
			timeText.text = "";
		}
		else
		{
			playImage.color = new Color(1f, 1f, 1f, 0.25f);
			timeText.text = GetTimeSpan(ELSingleton<IapManager>.Instance.GetRewardedVideoTimeSpan() + 1);
		}
	}

	public unsafe static string GetTimeSpan(long t)
	{
		long num = t / 3600;
		t %= 3600;
		long num2 = t / 60;
		t %= 60;
		return ((int*)(&num))->ToString("00") + ":" + ((int*)(&num2))->ToString("00") + ":" + ((int*)(&t))->ToString("00");
	}

	public void PlayButton()
	{
		if (!ELSingleton<AdsManager>.Instance.IsRewardedVideoAd)
		{
			ELSingleton<MessagePopUp>.Instance.ShowPopUp("Shop", "Please check your Internet connection and try again later.\nVideo is not available.");
		}
		if (ELSingleton<IapManager>.Instance.ShowRewardedVideo(aIsExecute: false))
		{
			ELSingleton<IapManager>.Instance.ShowRewardedVideo(aIsExecute: true);
		}
	}

	public void CheckVisible()
	{
		Vector3 vector = Camera.main.WorldToViewportPoint(base.gameObject.transform.position);
		bool flag = vector.y >= -0.1f && vector.y <= 1.1f;
		if (flag != isVisible)
		{
			base.gameObject.SetActive(flag);
		}
		isVisible = flag;
	}

	private void OnEnable()
	{
		CommonButton[] componentsInChildren = base.gameObject.GetComponentsInChildren<CommonButton>(includeInactive: true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Show();
		}
	}
}
