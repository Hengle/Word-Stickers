using UnityEngine;
using UnityEngine.UI;

public class GameBackground : MonoBehaviour
{
	public Image fill;

	public int amount;

	public bool[] hasLadybug;

	private int index;

	private void Awake()
	{
		fill.sprite = null;
		index = int.MaxValue;
	}

	public void Reset(LevelType aLevelType, int aPackIndex)
	{
		switch (aLevelType)
		{
		case LevelType.Normal:
			if (aPackIndex >= 0 && index != aPackIndex)
			{
				fill.sprite = Resources.Load<Sprite>($"Game/ingame-background{aPackIndex % amount:D2}");
				index = aPackIndex;
				Resources.UnloadUnusedAssets();
			}
			break;
		case LevelType.BonusRound:
			if (index != -1)
			{
				fill.sprite = Resources.Load<Sprite>("Game/special-background");
				index = -1;
				Resources.UnloadUnusedAssets();
			}
			break;
		case LevelType.DailyPuzzle:
			if (index != -2)
			{
				fill.sprite = Resources.Load<Sprite>("Game/special-background");
				index = -2;
				Resources.UnloadUnusedAssets();
			}
			break;
		}
		if (base.isActiveAndEnabled)
		{
			Animator component = base.gameObject.GetComponent<Animator>();
			component.Play("Idle");
			component.enabled = true;
		}
	}

	public bool HasLadyBug()
	{
		if (index >= 0 && index < hasLadybug.Length)
		{
			return hasLadybug[index];
		}
		return false;
	}

	public void StartAnimation()
	{
		base.gameObject.GetComponent<Animator>().enabled = true;
	}

	public void StopAnimation()
	{
		base.gameObject.GetComponent<Animator>().enabled = false;
	}
}
