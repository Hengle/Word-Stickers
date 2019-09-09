using System;
using UnityEngine;
using UnityEngine.UI;

public class CommonCoin : MonoBehaviour
{
	public enum CoinType
	{
		Coin,
		ExtraWord,
		Hint
	}

	public enum CoinState
	{
		SpreadLinearly,
		SpreadRadial,
		Idle,
		Fly
	}

	public Image coin;

	public Image extraWord;

	public ParticleSystem particle;

	public float spreadTime;

	public float spreadX;

	public float spreadY;

	public AnimationCurve spreadCurve;

	public float idleTimeMin;

	public float idleTimeMax;

	public float flyTime;

	public AnimationCurve flyCurve;

	public float flyY;

	public AnimationCurve flyCurveY;

	public AudioClip soundShow;

	public AudioClip soundHit;

	private Vector3 positionStart;

	private Vector3 positionSpread;

	private Vector3 positionTarget;

	private float scale;

	private CoinType type;

	private CoinState state;

	private float delay;

	private float time;

	private bool isFullUpdate;

	private static bool isSoundShow;

	private static bool isSoundHit;

	public Vector3 PositionStart
	{
		set
		{
			positionStart = value;
		}
	}

	public Vector3 PositionTarget
	{
		set
		{
			positionTarget = value;
		}
	}

	public float Scale
	{
		set
		{
			scale = value;
		}
	}

	public CoinType Type
	{
		set
		{
			type = value;
		}
	}

	public CoinState State
	{
		set
		{
			state = value;
		}
	}

	public float Delay
	{
		set
		{
			delay = value;
		}
	}

	public bool IsFullUpdate
	{
		set
		{
			isFullUpdate = value;
		}
	}

	private void Awake()
	{
		coin.gameObject.SetActive(value: false);
		extraWord.gameObject.SetActive(value: false);
		time = 0f;
	}

	private void Start()
	{
		base.gameObject.transform.position = positionStart;
		if (state == CoinState.SpreadLinearly)
		{
			positionSpread = new Vector3(base.gameObject.transform.position.x + UnityEngine.Random.Range(0f - spreadX, spreadX), base.gameObject.transform.position.y + UnityEngine.Random.Range(0f - spreadY, spreadY), base.gameObject.transform.position.z);
		}
		else if (state == CoinState.SpreadRadial)
		{
			float f = UnityEngine.Random.Range(0f, (float)Math.PI * 2f);
			positionSpread = new Vector3(base.gameObject.transform.position.x + UnityEngine.Random.Range(spreadX * 0.6f, spreadX) * Mathf.Sin(f), base.gameObject.transform.position.y + UnityEngine.Random.Range(spreadY * 0.6f, spreadY) * Mathf.Cos(f), base.gameObject.transform.position.z);
		}
		particle.Play();
	}

	private void Update()
	{
		if (delay <= 0f)
		{
			switch (state)
			{
			case CoinState.SpreadLinearly:
			case CoinState.SpreadRadial:
			{
				if (type == CoinType.Coin || type == CoinType.Hint)
				{
					coin.gameObject.SetActive(value: true);
					extraWord.gameObject.SetActive(value: false);
				}
				else if (type == CoinType.ExtraWord)
				{
					coin.gameObject.SetActive(value: false);
					extraWord.gameObject.SetActive(value: true);
				}
				float num3 = Mathf.Min(time / spreadTime);
				base.transform.position = Vector3.Lerp(positionStart, positionSpread, spreadCurve.Evaluate(num3));
				base.transform.localScale = new Vector3(scale, scale, 1f);
				if (time <= 0f && !isSoundShow)
				{
					isSoundShow = true;
					ELSingleton<AudioManager>.Instance.PlaySfx(soundShow);
				}
				if (num3 >= 1f)
				{
					positionStart = base.transform.position;
					state = CoinState.Idle;
					delay = 0f;
					time = 0f;
				}
				break;
			}
			case CoinState.Idle:
				state = CoinState.Fly;
				delay = UnityEngine.Random.Range(idleTimeMin, idleTimeMax);
				time = 0f;
				break;
			case CoinState.Fly:
			{
				float num = Mathf.Min(time / flyTime);
				Vector3 position = Vector3.Lerp(positionStart, positionTarget, flyCurve.Evaluate(num));
				position.y += flyCurveY.Evaluate(num) * flyY;
				base.transform.position = position;
				float num2 = scale + (1f - scale) * flyCurve.Evaluate(num);
				base.transform.localScale = new Vector3(num2, num2, 1f);
				if (!(num >= 1f))
				{
					break;
				}
				if (type == CoinType.Coin)
				{
					if (ELSingleton<GameWindow>.Instance.isActiveAndEnabled)
					{
						ELSingleton<GameWindow>.Instance.shopButton.AddCoins((!isFullUpdate) ? 1 : int.MaxValue);
						ELSingleton<GameWindow>.Instance.shopButton.Pop(aIsWithParticles: true);
					}
					if (ELSingleton<MenuWindow>.Instance.isActiveAndEnabled)
					{
						ELSingleton<MenuWindow>.Instance.shopButton.AddCoins((!isFullUpdate) ? 1 : int.MaxValue);
						ELSingleton<MenuWindow>.Instance.shopButton.Pop(aIsWithParticles: true);
					}
					if (ELSingleton<PackWindow>.Instance.isActiveAndEnabled)
					{
						ELSingleton<PackWindow>.Instance.shopButton.AddCoins((!isFullUpdate) ? 1 : int.MaxValue);
						ELSingleton<PackWindow>.Instance.shopButton.Pop(aIsWithParticles: true);
					}
				}
				else if (type == CoinType.ExtraWord && ELSingleton<GameWindow>.Instance.isActiveAndEnabled)
				{
					ELSingleton<GameWindow>.Instance.extraWordsButton.Coin();
					ELSingleton<GameWindow>.Instance.extraWordsButton.CheckFull();
				}
				UnityEngine.Object.Destroy(base.gameObject);
				if (!isSoundHit)
				{
					isSoundHit = true;
					ELSingleton<AudioManager>.Instance.PlaySfx(soundHit);
				}
				break;
			}
			}
			time += Time.deltaTime;
		}
		else
		{
			delay -= Time.deltaTime;
		}
	}

	private void LateUpdate()
	{
		isSoundShow = false;
		isSoundHit = false;
	}
}
