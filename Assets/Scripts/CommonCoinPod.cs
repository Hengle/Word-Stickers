using UnityEngine;

public class CommonCoinPod : MonoBehaviour
{
	public CommonCoin commonCoinPrefab;

	public void Reset()
	{
		CommonCoin[] componentsInChildren = base.gameObject.GetComponentsInChildren<CommonCoin>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			UnityEngine.Object.Destroy(componentsInChildren[i].gameObject);
		}
	}

	public void ReleaseCoinsLinearly(int aNumberOfCoins, Vector3 aPositionStart, Vector3 aPositionTarget, float aScale, float aSpreadX, float aSpreadY, float aDelay, bool aIsFullUpdate, bool aIsWithParticles)
	{
		for (int i = 0; i < aNumberOfCoins; i++)
		{
			CommonCoin commonCoin = UnityEngine.Object.Instantiate(commonCoinPrefab, base.transform);
			commonCoin.Type = CommonCoin.CoinType.Coin;
			commonCoin.State = CommonCoin.CoinState.SpreadLinearly;
			commonCoin.PositionStart = aPositionStart;
			commonCoin.PositionTarget = aPositionTarget;
			commonCoin.Scale = aScale;
			commonCoin.Delay = aDelay;
			commonCoin.IsFullUpdate = aIsFullUpdate;
			commonCoin.spreadX = aSpreadX;
			commonCoin.spreadY = aSpreadY;
			commonCoin.particle.gameObject.SetActive(aIsWithParticles);
		}
	}

	public void ReleaseCoinsRadialCustomImage(Sprite aImage, int aNumberOfCoins, Vector3 aPositionStart, Vector3 aPositionTarget, float aScale, float aSpreadX, float aSpreadY, float aDelay)
	{
		for (int i = 0; i < aNumberOfCoins; i++)
		{
			CommonCoin commonCoin = UnityEngine.Object.Instantiate(commonCoinPrefab, base.transform);
			commonCoin.Type = CommonCoin.CoinType.Hint;
			commonCoin.State = CommonCoin.CoinState.SpreadRadial;
			commonCoin.PositionStart = aPositionStart;
			commonCoin.PositionTarget = aPositionTarget;
			commonCoin.Scale = aScale;
			commonCoin.Delay = aDelay;
			commonCoin.IsFullUpdate = false;
			commonCoin.spreadX = aSpreadX;
			commonCoin.spreadY = aSpreadY;
			commonCoin.particle.gameObject.SetActive(value: false);
			commonCoin.coin.sprite = aImage;
		}
	}

	public void ReleaseCoinsRadial(int aNumberOfCoins, Vector3 aPositionStart, Vector3 aPositionTarget, float aScale, float aSpreadX, float aSpreadY, float aDelay, bool aIsFullUpdate, bool aIsWithParticles)
	{
		for (int i = 0; i < aNumberOfCoins; i++)
		{
			CommonCoin commonCoin = UnityEngine.Object.Instantiate(commonCoinPrefab, base.transform);
			commonCoin.Type = CommonCoin.CoinType.Coin;
			commonCoin.State = CommonCoin.CoinState.SpreadRadial;
			commonCoin.PositionStart = aPositionStart;
			commonCoin.PositionTarget = aPositionTarget;
			commonCoin.Scale = aScale;
			commonCoin.Delay = aDelay;
			commonCoin.IsFullUpdate = aIsFullUpdate;
			commonCoin.spreadX = aSpreadX;
			commonCoin.spreadY = aSpreadY;
			commonCoin.particle.gameObject.SetActive(aIsWithParticles);
		}
	}

	public void ReleaseExtraWord(Vector3 aPositionStart, Vector3 aPositionTarget, float aScale, float aDelay = 0f)
	{
		CommonCoin commonCoin = UnityEngine.Object.Instantiate(commonCoinPrefab, base.transform);
		commonCoin.Type = CommonCoin.CoinType.ExtraWord;
		commonCoin.State = CommonCoin.CoinState.SpreadRadial;
		commonCoin.PositionStart = aPositionStart;
		commonCoin.PositionTarget = aPositionTarget;
		commonCoin.Scale = aScale;
		commonCoin.Delay = aDelay;
		commonCoin.IsFullUpdate = false;
		commonCoin.spreadX = 0.5f;
		commonCoin.spreadY = 0.5f;
		commonCoin.particle.gameObject.SetActive(value: true);
	}

	public bool HasCoins()
	{
		return base.gameObject.GetComponentsInChildren<CommonCoin>().Length != 0;
	}
}
