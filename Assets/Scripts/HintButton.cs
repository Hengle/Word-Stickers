using System;
using UnityEngine;
using UnityEngine.UI;

public class HintButton : CommonButton
{
	private const float EPSILON = 0.01f;

	private const float TIME = 0.5f;

	public LevelHint type;

	public GameObject coin;

	public GameObject price;

	public GameObject plus;

	public GameObject empty;

	public GameObject amount;

	private Vector3 positionStart;

	private Vector3 positionTarget;

	private float time;

	public float PositionTargetX
	{
		set
		{
			positionStart = base.transform.localPosition;
			positionTarget = new Vector3(value, base.transform.localPosition.y, 0f);
		}
	}

	public Vector3 PositionTarget => positionTarget;

	private void Update()
	{
		if (Math.Abs(base.transform.localPosition.x - positionTarget.x) > 0.01f)
		{
			float t = Mathf.Min(time / 0.5f);
			base.transform.localPosition = Vector3.Lerp(positionStart, positionTarget, t);
			time += Time.deltaTime;
		}
		else
		{
			time = 0f;
		}
	}

	public new void Reset()
	{
		base.Reset();
		base.transform.localPosition = new Vector3(0f, base.transform.localPosition.y, 0f);
		positionStart = base.transform.localPosition;
		positionTarget = base.transform.localPosition;
		Setup();
	}

	public void Setup()
	{
		if (ELSingleton<HintManager>.Instance.GetAmount(type) > 0)
		{
			coin.SetActive(value: false);
			price.SetActive(value: false);
			plus.SetActive(value: false);
			empty.SetActive(value: true);
			amount.SetActive(value: true);
			amount.GetComponent<Text>().text = ELSingleton<HintManager>.Instance.GetAmount(type).ToString();
		}
		else if (ELSingleton<HintManager>.Instance.GetCoins(type) > 0 && ELSingleton<CoinsManager>.Instance.Coins < ELSingleton<HintManager>.Instance.GetCoins(type))
		{
			coin.SetActive(value: true);
			price.SetActive(value: true);
			plus.SetActive(value: true);
			empty.SetActive(value: false);
			amount.SetActive(value: false);
			price.GetComponent<Text>().text = ELSingleton<HintManager>.Instance.GetCoins(type).ToString();
		}
		else if (ELSingleton<HintManager>.Instance.GetCoins(type) > 0)
		{
			coin.SetActive(value: true);
			price.SetActive(value: true);
			plus.SetActive(value: false);
			empty.SetActive(value: false);
			amount.SetActive(value: false);
			price.GetComponent<Text>().text = ELSingleton<HintManager>.Instance.GetCoins(type).ToString();
		}
		else
		{
			coin.SetActive(value: false);
			price.SetActive(value: false);
			plus.SetActive(value: true);
			empty.SetActive(value: false);
			amount.SetActive(value: false);
		}
	}
}
