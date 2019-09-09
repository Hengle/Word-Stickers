using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PackItem : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IPointerClickHandler
{
	public const float PACK_TEM_HEIGHT_CLOSED = 230f;

	public const float PACK_TEM_HEIGHT_OPEN = 850f;

	public Text packName;

	public Text packIndex;

	public UnityEvent OnClick;

	public UnityEvent OnFocus;

	public Animator packAnimator;

	public RectTransform backRect;

	public GameObject check;

	public GameObject content;

	public GameObject badge;

	public PackLevelItem packLevelItemPrefab;

	public AudioClip soundButtonGeneral;

	[HideInInspector]
	public LevelInfo levelInfo;

	[HideInInspector]
	public float normalizedScrollLocation;

	private bool isVisible = true;

	private bool isOpen;

	public bool IsUnlocked
	{
		get;
		private set;
	}

	public bool IsCurrent
	{
		get;
		private set;
	}

	public bool IsLast
	{
		get;
		private set;
	}

	public void UpdateState(LevelInfo info)
	{
		int num = levelInfo.ComparePack(info);
		IsUnlocked = (num >= 0);
		IsCurrent = (num == 0);
	}

	public void UpdateLook()
	{
		check.SetActive(IsUnlocked && !IsCurrent);
		badge.SetActive(IsUnlocked && !IsCurrent);
	}

	public float Prepare(LevelInfo aLevelInfo, int index, PackWindow.AnimationState state)
	{
		float result = 0f;
		normalizedScrollLocation = 1f;
		levelInfo = aLevelInfo;
		Pack pack = ELSingleton<LevelsSettings>.Instance.levelSet.GetPack(aLevelInfo);
		packName.text = pack.name;
		packIndex.text = string.Concat(index + 1);
		LevelInfo info = ELSingleton<LevelsSettings>.Instance.levelSet.GetFirstNotCompleateLevel();
		if (state == PackWindow.AnimationState.ANIMATION_STATE_PACK_WAIT)
		{
			info = ELSingleton<LevelsSettings>.Instance.levelSet.GetPreviousPackCompleteInfo(info);
		}
		UpdateState(info);
		if (IsCurrent)
		{
			result = SetupLevels(state);
		}
		packAnimator.SetBool("Locked", !IsUnlocked);
		packAnimator.SetBool("Open", IsCurrent);
		packAnimator.SetTrigger("Init");
		UpdateLook();
		if (state == PackWindow.AnimationState.ANIMATION_STATE_PACK_WAIT)
		{
			UpdateState(ELSingleton<LevelsSettings>.Instance.levelSet.GetFirstNotCompleateLevel());
		}
		return result;
	}

	public void OnPointerDown(PointerEventData aEventData)
	{
	}

	public void OnPointerUp(PointerEventData aEventData)
	{
	}

	public virtual void OnPointerClick(PointerEventData aEventData)
	{
		if (ELSingleton<PackWindow>.Instance.actionTurnOff)
		{
			return;
		}
		bool flag = !packAnimator.GetBool("Open");
		if (flag)
		{
			if (IsUnlocked)
			{
				packAnimator.SetBool("Open", flag);
				packAnimator.SetTrigger("Toggle");
				OnFocus.Invoke();
				SetupLevels();
			}
			else if (ELSingleton<ApplicationSettings>.Instance.DeploymentSettings.isCheatsEnabled)
			{
				packAnimator.SetBool("Open", flag);
				packAnimator.SetTrigger("Cheat");
				packAnimator.SetTrigger("Toggle");
				OnFocus.Invoke();
				SetupLevels();
			}
		}
		else
		{
			OnClick.Invoke();
		}
	}

	public void OpenAfterComplete()
	{
		UpdateLook();
		packAnimator.SetBool("Open", value: true);
		packAnimator.SetTrigger("Cheat");
		packAnimator.SetTrigger("Toggle");
		OnFocus.Invoke();
		SetupLevels();
	}

	public void CloseItem()
	{
		packAnimator.SetBool("Open", value: false);
		packAnimator.SetTrigger("Toggle");
		PackLevelItem[] componentsInChildren = content.GetComponentsInChildren<PackLevelItem>();
		foreach (PackLevelItem obj in componentsInChildren)
		{
			obj.Close();
			obj.CancelInvoke();
		}
	}

	public float SetupLevels(PackWindow.AnimationState state = PackWindow.AnimationState.ANIMATION_STATE_NONE)
	{
		foreach (Transform item in content.transform)
		{
			item.gameObject.SetActive(value: false);
			UnityEngine.Object.Destroy(item.gameObject);
		}
		Pack pack = ELSingleton<LevelsSettings>.Instance.levelSet.GetPack(levelInfo);
		List<PackLevelItem> list = new List<PackLevelItem>();
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		int num4 = -1;
		for (int i = 0; i < pack.levels.Count; i++)
		{
			LevelInfo li = levelInfo;
			li.currentLevel = i;
			PackLevelItem packLevelItem = UnityEngine.Object.Instantiate(packLevelItemPrefab, content.transform);
			packLevelItem.Prepare(li, num3, i);
			if (packLevelItem.IsCompleteAnimation)
			{
				num4 = i;
			}
			RectTransform component = packLevelItem.GetComponent<RectTransform>();
			packLevelItem.transform.localPosition = new Vector2(num - component.sizeDelta.x * 2f, 0f - num2 - component.sizeDelta.y / 2f + 20f);
			num += component.sizeDelta.x;
			if (i % 5 == 4)
			{
				num = 0f;
				num2 += component.sizeDelta.y;
			}
			num3 += 0.02f;
			if (packLevelItem.IsUnlocked || ELSingleton<ApplicationSettings>.Instance.DeploymentSettings.isCheatsEnabled)
			{
				packLevelItem.OnClick.AddListener(delegate
				{
					PlayButton(li);
				});
			}
			list.Add(packLevelItem);
		}
		int num5 = 0;
		float num6 = 0f;
		foreach (PackLevelItem item2 in list)
		{
			if (!item2.IsCompleteAnimation)
			{
				item2.StartAnimation(num6, old: false);
			}
			num6 += 0.02f;
			num5++;
		}
		num5 = 0;
		num6 = 0.5f;
		foreach (PackLevelItem item3 in list)
		{
			bool flag = num4 - num5 >= 5;
			float num7 = flag ? 0.1f : 0.5f;
			if (flag)
			{
				item3.particle = null;
			}
			if (item3.IsCompleteAnimation)
			{
				item3.StartAnimation(num6, flag);
				num6 += num7;
				item3.transform.SetAsLastSibling();
			}
			if (item3.IsCurrent)
			{
				item3.StartPlayTransform(num6);
			}
			num5++;
		}
		pack.progressIndex = Mathf.Max(num4 + 1, pack.progressIndex);
		ELSingleton<LevelsSettings>.Instance.Save();
		return num6;
	}

	public void PlayButton(LevelInfo li)
	{
		if (!ELSingleton<PackWindow>.Instance.actionTurnOff)
		{
			ELSingleton<LevelsSettings>.Instance.levelSet.SetCurrentLevel(li);
			ELSingleton<GameWindow>.Instance.SetLoad(LevelType.Normal, ELSingleton<LevelsSettings>.Instance.levelSet.GetCurrentPack().index);
			ELSingleton<ApplicationManager>.Instance.ShowGameAfterPack();
			ELSingleton<AudioManager>.Instance.PlaySfx(soundButtonGeneral);
		}
	}

	public void CheckVisible()
	{
		isOpen |= packAnimator.GetBool("Open");
		isOpen &= !packAnimator.GetCurrentAnimatorStateInfo(0).IsName("Close");
		Vector3 vector = Camera.main.WorldToViewportPoint(base.gameObject.transform.position);
		bool flag = (vector.y >= -0.1f && vector.y <= 1.1f) || isOpen;
		if (flag != isVisible)
		{
			base.gameObject.SetActive(flag);
		}
		isVisible = flag;
	}

	private void OnEnable()
	{
		packAnimator.SetBool("Locked", !IsUnlocked);
		packAnimator.SetTrigger("Init");
		UpdateLook();
	}
}
