using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PackLevelItem : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IPointerClickHandler
{
	public const float PACK_LEVEL_ITEM_SIZE = 200f;

	public UnityEvent OnClick;

	public Image icon;

	[HideInInspector]
	public LevelInfo levelInfo;

	public Animator animator;

	public Image circle;

	public GameObject check;

	public Sprite questionSprite;

	public Sprite playSprite;

	public ParticleSystem particle;

	public AudioClip unlockSound;

	private bool isUnlocked;

	private bool isCompleteAnimation;

	private bool isCurrent;

	public bool IsUnlocked => isUnlocked;

	public bool IsCompleteAnimation => isCompleteAnimation;

	public bool IsCurrent => isCurrent;

	public void Prepare(LevelInfo aLevelInfo, float delay, int index)
	{
		levelInfo = aLevelInfo;
		Pack pack = ELSingleton<LevelsSettings>.Instance.levelSet.GetPack(levelInfo);
		Level level = ELSingleton<LevelsSettings>.Instance.levelSet.GetLevel(levelInfo);
		ELSingleton<IconsManager>.Instance.Load(pack.name);
		icon.sprite = Sprite.Create(ELSingleton<IconsManager>.Instance.Icon.texture, new Rect(level.iconRect.x, (float)ELSingleton<IconsManager>.Instance.Icon.texture.height - level.iconRect.y - level.iconRect.height, level.iconRect.width, level.iconRect.height), new Vector2(0f, 0f));
		icon.SetNativeSize();
		int num = aLevelInfo.Compare(ELSingleton<LevelsSettings>.Instance.levelSet.GetFirstNotCompleateLevel());
		isCurrent = (num == 0);
		isUnlocked = (num >= 0);
		isCompleteAnimation = false;
		if (isUnlocked && !isCurrent && index >= pack.progressIndex)
		{
			isCompleteAnimation = true;
		}
		else if (isCurrent)
		{
			circle.sprite = questionSprite;
			icon.gameObject.SetActive(value: false);
			check.SetActive(value: false);
		}
		else if (!isUnlocked)
		{
			circle.sprite = questionSprite;
			icon.gameObject.SetActive(value: false);
			check.SetActive(value: false);
		}
	}

	public void StartAnimation(float delay, bool old)
	{
		ELSingleton<LevelsSettings>.Instance.levelSet.GetPack(levelInfo);
		if (isCompleteAnimation)
		{
			Invoke("PrepareCompleteInvoke", delay);
			if (!old)
			{
				Invoke("PrepareCompleteInvokeSfx", Mathf.Max(0f, delay + 0.5f));
			}
		}
		else
		{
			Invoke("PrepareInvoke", delay);
		}
		if (isCurrent)
		{
			animator.SetBool("PlayIdle", value: true);
		}
	}

	public void PrepareInvoke()
	{
		animator.SetBool("Open", value: true);
	}

	public void PrepareCompleteInvokeSfx()
	{
		ELSingleton<AudioManager>.Instance.PlaySfx(unlockSound);
	}

	public void PrepareCompleteInvoke()
	{
		animator.SetBool("Complete", value: true);
		animator.SetBool("Open", value: true);
	}

	public void Kill()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void Close()
	{
		animator.SetBool("Open", value: false);
	}

	public void OnPointerDown(PointerEventData aEventData)
	{
	}

	public void OnPointerUp(PointerEventData aEventData)
	{
	}

	public virtual void OnPointerClick(PointerEventData aEventData)
	{
		OnClick.Invoke();
	}

	public void StartParticle()
	{
		if ((bool)particle)
		{
			particle.Emit(1);
			particle.Play();
		}
	}

	public void StartPlayTransform(float time)
	{
		Invoke("StartPlayTransformInvoke", time);
	}

	public void StartPlayTransformInvoke()
	{
		animator.SetTrigger("PlayTransform");
	}

	public void UpdatePlaySprite()
	{
		circle.sprite = playSprite;
	}
}
