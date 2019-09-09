using UnityEngine;
using UnityEngine.UI;

public class BoardLetter : MonoBehaviour
{
	public enum Type
	{
		Normal,
		Coin
	}

	public enum Order
	{
		First,
		Middle,
		Last
	}

	public enum HintDirection
	{
		None,
		Left,
		Right,
		Up,
		Down
	}

	public enum SelectorDirection
	{
		None,
		Left,
		Right,
		Up,
		Down
	}

	private static string[] alphabet = new string[98]
	{
		"E",
		"E",
		"E",
		"E",
		"E",
		"E",
		"E",
		"E",
		"E",
		"E",
		"E",
		"E",
		"A",
		"A",
		"A",
		"A",
		"A",
		"A",
		"A",
		"A",
		"A",
		"I",
		"I",
		"I",
		"I",
		"I",
		"I",
		"I",
		"I",
		"I",
		"O",
		"O",
		"O",
		"O",
		"O",
		"O",
		"O",
		"O",
		"N",
		"N",
		"N",
		"N",
		"N",
		"N",
		"R",
		"R",
		"R",
		"R",
		"R",
		"R",
		"T",
		"T",
		"T",
		"T",
		"T",
		"T",
		"L",
		"L",
		"L",
		"L",
		"S",
		"S",
		"S",
		"S",
		"U",
		"U",
		"U",
		"U",
		"D",
		"D",
		"D",
		"D",
		"G",
		"G",
		"G",
		"B",
		"B",
		"C",
		"C",
		"M",
		"M",
		"P",
		"P",
		"F",
		"F",
		"H",
		"H",
		"V",
		"V",
		"W",
		"W",
		"Y",
		"Y",
		"K",
		"J",
		"X",
		"Q",
		"Z"
	};

	public BoxCollider2D boxCollider;

	public GameObject tile;

	public Image tileShine;

	public GameObject tileSelected;

	public GameObject hint;

	public Image hintDotFirst;

	public Image hintDotOther;

	public Image hintLeft;

	public Image hintRight;

	public Image hintUp;

	public Image hintDown;

	public GameObject selector;

	public GameObject selectorLeft;

	public GameObject selectorRight;

	public GameObject selectorUp;

	public GameObject selectorDown;

	public GameObject letter;

	public GameObject letterSelected;

	public GameObject coin;

	public GameObject particlesMark;

	public GameObject particlesHint;

	public AudioClip soundSelectedNormal;

	public AudioClip soundSelectedCoin;

	public AudioClip soundUnselected;

	private LevelType levelType;

	private Type type;

	private Order order;

	private LevelHint hintType;

	private HintDirection hintDirection;

	private bool isHintParticle;

	private bool isShining;

	private float shiningCounter;

	public float shiningTime;

	public float shiningWidth;

	public Color goodStartColor;

	public Color startAndFinishColor;

	public Color exposeColor;

	public bool IsAvailable
	{
		get;
		set;
	}

	public string Letter
	{
		get;
		private set;
	}

	public int X
	{
		get;
		private set;
	}

	public int Y
	{
		get;
		private set;
	}

	public bool IsSelected
	{
		get;
		private set;
	}

	public int SelectedIndex
	{
		get;
		set;
	}

	public bool IsMarked
	{
		get;
		private set;
	}

	private void Update()
	{
		if (isShining)
		{
			if (shiningCounter <= shiningTime)
			{
				float num = 1f / shiningTime;
				shiningCounter += Time.deltaTime;
				float value = Mathf.Lerp(0f, 1f, num * shiningCounter);
				tileShine.material.SetFloat("_TimeController", value);
			}
			else
			{
				tileShine.gameObject.SetActive(value: false);
				tileShine.material.SetFloat("_Width", 0f);
				isShining = false;
			}
		}
	}

	public void Set(LevelLetter aLevelLetter, float aWordLetterOffset, Order aOrder, HintDirection aHintDirection, bool aIsLevelCompleted, LevelType aLevelType)
	{
		boxCollider.size = new Vector2(aWordLetterOffset, aWordLetterOffset);
		IsAvailable = true;
		levelType = aLevelType;
		Letter = aLevelLetter.letter.ToUpper();
		type = (((!aIsLevelCompleted && aLevelLetter.isCoin) || aLevelType == LevelType.BonusRound || aLevelType == LevelType.DailyPuzzle) ? Type.Coin : Type.Normal);
		order = aOrder;
		X = aLevelLetter.x;
		Y = aLevelLetter.y;
		IsSelected = false;
		SelectedIndex = -1;
		IsMarked = false;
		hintType = LevelHint.None;
		hintDirection = aHintDirection;
		isHintParticle = true;
		isShining = false;
		shiningCounter = 0f;
		base.transform.localPosition = new Vector3(((float)X + 0.5f) * aWordLetterOffset, (0f - ((float)Y + 0.5f)) * aWordLetterOffset, 0f);
		tile.GetComponent<Canvas>().sortingOrder = X + Y + 1;
		tileShine.material = new Material(tileShine.material);
		tileShine.gameObject.SetActive(value: false);
		tileSelected.SetActive(value: false);
		hint.SetActive(value: false);
		if (order == Order.First)
		{
			hintDotFirst.gameObject.SetActive(value: true);
			hintDotOther.gameObject.SetActive(value: false);
		}
		else if (order == Order.Last)
		{
			hintDotFirst.gameObject.SetActive(value: false);
			hintDotOther.gameObject.SetActive(value: true);
		}
		else
		{
			hintDotFirst.gameObject.SetActive(value: false);
			hintDotOther.gameObject.SetActive(value: true);
		}
		selector.SetActive(value: false);
		selectorLeft.SetActive(value: false);
		selectorRight.SetActive(value: false);
		selectorUp.SetActive(value: false);
		selectorDown.SetActive(value: false);
		Text[] componentsInChildren = letter.GetComponentsInChildren<Text>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].text = Letter;
		}
		letter.SetActive(value: true);
		letterSelected.SetActive(value: false);
		if (type == Type.Normal)
		{
			coin.SetActive(value: false);
		}
		else if (type == Type.Coin)
		{
			coin.SetActive(value: true);
		}
	}

	public bool HasCoin()
	{
		return type == Type.Coin;
	}

	public void RewardCoin()
	{
		if (type == Type.Coin)
		{
			ELSingleton<CoinsManager>.Instance.AddCoins(1);
			ELSingleton<GameWindow>.Instance.StatsAddCoins(1);
		}
	}

	public void Show(float aDelay = 0f)
	{
		Invoke("ShowInvoke", aDelay);
	}

	private void ShowInvoke()
	{
		if (base.isActiveAndEnabled && IsAvailable)
		{
			base.gameObject.GetComponent<Animator>().SetTrigger("Show");
		}
	}

	private void ShowEnded()
	{
		base.gameObject.GetComponent<Animator>().SetBool("IsIdle", value: true);
	}

	public void Hide(float aDelay = 0f)
	{
		Invoke("HideInvoke", aDelay);
	}

	private void HideInvoke()
	{
		if (base.isActiveAndEnabled)
		{
			base.gameObject.GetComponent<Animator>().SetTrigger("Hide");
		}
	}

	public void HideEnded()
	{
		tile.SetActive(value: false);
		hint.SetActive(value: false);
		selector.SetActive(value: false);
		letter.SetActive(value: false);
		coin.SetActive(value: false);
	}

	public void Mark(bool aIsWithAnimation)
	{
		IsMarked = true;
		if (aIsWithAnimation)
		{
			Animator component = base.gameObject.GetComponent<Animator>();
			component.SetTrigger("Mark");
			component.SetBool("IsIdle", value: false);
		}
		else
		{
			MarkEnded();
		}
	}

	public void MarkReleaseCoin()
	{
		if (type == Type.Coin)
		{
			ELSingleton<GameWindow>.Instance.coinPod.ReleaseCoinsLinearly(1, base.transform.position, ELSingleton<GameWindow>.Instance.shopButton.coinTarget.transform.position, 2f, 0.5f, 0.5f, 0f, aIsFullUpdate: false, aIsWithParticles: true);
		}
		type = Type.Normal;
	}

	public void MarkEnded()
	{
		if (levelType == LevelType.Normal)
		{
			tile.SetActive(value: false);
			hint.SetActive(value: false);
			selector.SetActive(value: false);
			letter.SetActive(value: false);
			coin.SetActive(value: false);
		}
		else if (levelType == LevelType.BonusRound)
		{
			Letter = alphabet[Random.Range(0, alphabet.Length)];
			IsMarked = false;
			type = Type.Coin;
			Text[] componentsInChildren = letter.GetComponentsInChildren<Text>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].text = Letter;
			}
			Show((float)SelectedIndex * 0.1f);
			UnSelect(aIsSound: false);
		}
	}

	public void Hint(LevelHint aLevelHint, bool aIsWithAnimation, float aDelay = 0f)
	{
		if (aLevelHint != 0 && !IsMarked)
		{
			hintType = aLevelHint;
			isHintParticle = aIsWithAnimation;
			if (!aIsWithAnimation)
			{
				HintEnded();
			}
			Invoke("HintInvoke", aDelay);
		}
	}

	private void HintInvoke()
	{
		if (base.isActiveAndEnabled)
		{
			hint.SetActive(value: true);
			if (hintDirection == HintDirection.None || hintType == LevelHint.GoodStart || hintType == LevelHint.StartAndFinish)
			{
				hintLeft.gameObject.SetActive(value: false);
				hintRight.gameObject.SetActive(value: false);
				hintUp.gameObject.SetActive(value: false);
				hintDown.gameObject.SetActive(value: false);
			}
			else if (hintDirection == HintDirection.Left)
			{
				hintLeft.gameObject.SetActive(value: true);
				hintRight.gameObject.SetActive(value: false);
				hintUp.gameObject.SetActive(value: false);
				hintDown.gameObject.SetActive(value: false);
			}
			else if (hintDirection == HintDirection.Right)
			{
				hintLeft.gameObject.SetActive(value: false);
				hintRight.gameObject.SetActive(value: true);
				hintUp.gameObject.SetActive(value: false);
				hintDown.gameObject.SetActive(value: false);
			}
			else if (hintDirection == HintDirection.Up)
			{
				hintLeft.gameObject.SetActive(value: false);
				hintRight.gameObject.SetActive(value: false);
				hintUp.gameObject.SetActive(value: true);
				hintDown.gameObject.SetActive(value: false);
			}
			else if (hintDirection == HintDirection.Down)
			{
				hintLeft.gameObject.SetActive(value: false);
				hintRight.gameObject.SetActive(value: false);
				hintUp.gameObject.SetActive(value: false);
				hintDown.gameObject.SetActive(value: true);
			}
			Color color = (hintType == LevelHint.GoodStart) ? goodStartColor : ((hintType == LevelHint.StartAndFinish) ? startAndFinishColor : ((hintType == LevelHint.Expose) ? exposeColor : Color.white));
			hintDotFirst.color = color;
			hintDotOther.color = color;
			hintLeft.color = color;
			hintRight.color = color;
			hintUp.color = color;
			hintDown.color = color;
			Animator component = base.gameObject.GetComponent<Animator>();
			component.Play("HintInit");
			component.SetTrigger("Hint");
		}
	}

	public void HintEnded()
	{
		hint.SetActive(value: true);
	}

	public void Select(int aIndex, SelectorDirection aSelectorDirection)
	{
		if (!IsMarked)
		{
			IsSelected = true;
			SelectedIndex = aIndex;
			tileSelected.SetActive(value: true);
			letterSelected.SetActive(value: true);
			selector.SetActive(value: true);
			switch (aSelectorDirection)
			{
			case SelectorDirection.None:
				selectorLeft.SetActive(value: false);
				selectorRight.SetActive(value: false);
				selectorUp.SetActive(value: false);
				selectorDown.SetActive(value: false);
				break;
			case SelectorDirection.Left:
				selectorLeft.SetActive(value: true);
				selectorRight.SetActive(value: false);
				selectorUp.SetActive(value: false);
				selectorDown.SetActive(value: false);
				break;
			case SelectorDirection.Right:
				selectorLeft.SetActive(value: false);
				selectorRight.SetActive(value: true);
				selectorUp.SetActive(value: false);
				selectorDown.SetActive(value: false);
				break;
			case SelectorDirection.Up:
				selectorLeft.SetActive(value: false);
				selectorRight.SetActive(value: false);
				selectorUp.SetActive(value: true);
				selectorDown.SetActive(value: false);
				break;
			case SelectorDirection.Down:
				selectorLeft.SetActive(value: false);
				selectorRight.SetActive(value: false);
				selectorUp.SetActive(value: false);
				selectorDown.SetActive(value: true);
				break;
			}
			Animator component = base.gameObject.GetComponent<Animator>();
			component.SetTrigger("Select");
			component.ResetTrigger("Unselect");
			if (type == Type.Normal)
			{
				ELSingleton<AudioManager>.Instance.PlaySfx(soundSelectedNormal);
			}
			else if (type == Type.Coin)
			{
				ELSingleton<AudioManager>.Instance.PlaySfx(soundSelectedCoin);
			}
		}
	}

	public void UnSelect(bool aIsSound, float aDelay = 0f)
	{
		IsSelected = false;
		SelectedIndex = -1;
		Animator component = base.gameObject.GetComponent<Animator>();
		component.SetTrigger("Unselect");
		component.ResetTrigger("Select");
		if (aIsSound)
		{
			ELSingleton<AudioManager>.Instance.PlaySfx(soundUnselected);
		}
	}

	public void UnSelectEnded()
	{
		if (!IsSelected)
		{
			tileSelected.SetActive(value: false);
			letterSelected.SetActive(value: false);
			selector.SetActive(value: false);
		}
	}

	public void Shine(float aDelay = 0f)
	{
		Invoke("ShineInvoke", aDelay);
	}

	private void ShineInvoke()
	{
		if (base.isActiveAndEnabled && !IsSelected && !IsMarked && !isShining)
		{
			isShining = true;
			shiningCounter = 0f;
			tileShine.gameObject.SetActive(value: true);
			tileShine.material.SetFloat("_Width", shiningWidth);
		}
	}

	public void ParticlesMark()
	{
		ParticleSystem[] componentsInChildren = particlesMark.GetComponentsInChildren<ParticleSystem>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Play();
		}
	}

	public void ParticlesHint()
	{
		ParticleSystem[] componentsInChildren = particlesHint.GetComponentsInChildren<ParticleSystem>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Play();
		}
	}
}
