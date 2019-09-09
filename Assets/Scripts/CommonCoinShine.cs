using UnityEngine;
using UnityEngine.UI;

public class CommonCoinShine : MonoBehaviour
{
	public Image[] coins;

	public float shiningTime;

	public float shiningWidth;

	private float shineDelay;

	private bool isShining;

	private float shiningCounter;

	private void Start()
	{
		for (int i = 0; i < coins.Length; i++)
		{
			if (coins[i].enabled)
			{
				coins[i].material = new Material(coins[i].material);
			}
		}
		shineDelay = UnityEngine.Random.Range(5f, 10f);
		isShining = false;
		shiningCounter = 0f;
	}

	private void Update()
	{
		if (shineDelay <= 0f)
		{
			shineDelay = UnityEngine.Random.Range(5f, 10f);
			for (int i = 0; i < coins.Length; i++)
			{
				if (coins[i].enabled)
				{
					isShining = true;
					shiningCounter = 0f;
					coins[i].material.SetFloat("_Width", shiningWidth);
				}
			}
		}
		else
		{
			shineDelay -= Time.deltaTime;
		}
		if (!isShining)
		{
			return;
		}
		if (shiningCounter <= shiningTime)
		{
			float num = 1f / shiningTime;
			shiningCounter += Time.deltaTime;
			float value = Mathf.Lerp(0f, 1f, num * shiningCounter);
			for (int j = 0; j < coins.Length; j++)
			{
				if (coins[j].enabled)
				{
					coins[j].material.SetFloat("_TimeController", value);
				}
			}
			return;
		}
		for (int k = 0; k < coins.Length; k++)
		{
			if (coins[k].enabled)
			{
				coins[k].material.SetFloat("_Width", 0f);
			}
		}
		isShining = false;
	}
}
