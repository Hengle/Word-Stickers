using UnityEngine;
using UnityEngine.UI;

public class ELDebug : MonoBehaviour
{
	public Text fpsText;

	public Text adsText;

	private float fpsDeltaTime;

	private void Start()
	{
		if (!ELSingleton<ApplicationSettings>.Instance.DeploymentSettings.isDebug)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void Update()
	{
		fpsDeltaTime += (Time.deltaTime - fpsDeltaTime) * 0.1f;
		float f = 1f / fpsDeltaTime;
		fpsText.text = Mathf.Ceil(f).ToString();
		adsText.text = $"i: {ELSingleton<AdsManager>.Instance.IsInterstitialAd}\nr: {ELSingleton<AdsManager>.Instance.IsRewardedVideoAd}";
	}
}
