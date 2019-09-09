using UnityEngine;

public class BoardWord : MonoBehaviour
{
	public BoardLetter boardLetterPrefab;

	private LevelWord levelWord;

	public bool IsCompleted
	{
		get
		{
			return levelWord.isCompleted;
		}
		set
		{
			levelWord.isCompleted = value;
		}
	}

	public string Word => levelWord.word;

	public LevelHint HintType => levelWord.hint;

	public void Set(LevelWord aWord, float aWordLetterOffset, bool aIsLevelCompleted, LevelType aLevelType)
	{
		levelWord = aWord;
		int num = int.MinValue;
		int num2 = int.MinValue;
		for (int i = 0; i < aWord.letters.Count; i++)
		{
			Object.Instantiate(boardLetterPrefab, base.transform).Set(aWord.letters[i], aWordLetterOffset, (i != 0) ? ((i != aWord.letters.Count - 1) ? BoardLetter.Order.Middle : BoardLetter.Order.Last) : BoardLetter.Order.First, (num == aWord.letters[i].x - 1) ? BoardLetter.HintDirection.Left : ((num == aWord.letters[i].x + 1) ? BoardLetter.HintDirection.Right : ((num2 == aWord.letters[i].y - 1) ? BoardLetter.HintDirection.Up : ((num2 == aWord.letters[i].y + 1) ? BoardLetter.HintDirection.Down : BoardLetter.HintDirection.None))), aIsLevelCompleted, aLevelType);
			num = aWord.letters[i].x;
			num2 = aWord.letters[i].y;
		}
		if (aWord.isCompleted)
		{
			IsCompleted = true;
			MarkLetters(aIsWithAnimation: false);
		}
		Hint(levelWord.hint, aIsNoneOnly: false, aIsForce: true, aIsWithAnimation: false);
	}

	public void RewardCoins()
	{
		BoardLetter[] componentsInChildren = base.gameObject.GetComponentsInChildren<BoardLetter>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].RewardCoin();
		}
	}

	public void ShowLetters(float aDelay = 0f)
	{
		Invoke("ShowLettersInvoke", aDelay);
	}

	private void ShowLettersInvoke()
	{
		if (base.isActiveAndEnabled)
		{
			BoardLetter[] componentsInChildren = base.gameObject.GetComponentsInChildren<BoardLetter>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].Show(UnityEngine.Random.Range(0f, 0.2f));
			}
		}
	}

	public void HideLetters(float aDelay = 0f)
	{
		Invoke("HideLettersInvoke", aDelay);
	}

	private void HideLettersInvoke()
	{
		if (base.isActiveAndEnabled)
		{
			BoardLetter[] componentsInChildren = base.gameObject.GetComponentsInChildren<BoardLetter>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].Hide(UnityEngine.Random.Range(0f, 0.1f));
			}
		}
	}

	public bool IsSelected()
	{
		BoardLetter[] componentsInChildren = base.gameObject.GetComponentsInChildren<BoardLetter>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (!componentsInChildren[i].IsSelected)
			{
				return false;
			}
		}
		return true;
	}

	public void UnSelectLetters(float aDelay = 0f)
	{
		Invoke("UnSelectLettersInvoke", aDelay);
	}

	private void UnSelectLettersInvoke()
	{
		if (!base.isActiveAndEnabled)
		{
			return;
		}
		BoardLetter[] componentsInChildren = base.gameObject.GetComponentsInChildren<BoardLetter>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i].IsSelected && !componentsInChildren[i].IsMarked)
			{
				componentsInChildren[i].UnSelect(aIsSound: false);
			}
		}
	}

	public void MarkLetters(bool aIsWithAnimation, float aDelay = 0f)
	{
		if (aIsWithAnimation)
		{
			Invoke("MarkLettersInvoke", aDelay);
		}
		else
		{
			MarkLettersExecute(aIsWithAnimation: false);
		}
	}

	private void MarkLettersInvoke()
	{
		if (base.isActiveAndEnabled)
		{
			MarkLettersExecute(aIsWithAnimation: true);
		}
	}

	private void MarkLettersExecute(bool aIsWithAnimation)
	{
		BoardLetter[] componentsInChildren = base.gameObject.GetComponentsInChildren<BoardLetter>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Mark(aIsWithAnimation);
		}
	}

	public bool Hint(LevelHint aLevelHint, bool aIsNoneOnly, bool aIsForce, bool aIsWithAnimation)
	{
		if (!levelWord.isCompleted && ((aLevelHint > levelWord.hint) | aIsForce) && (!aIsNoneOnly || levelWord.hint == LevelHint.None))
		{
			levelWord.hint = aLevelHint;
			BoardLetter[] componentsInChildren = base.gameObject.GetComponentsInChildren<BoardLetter>();
			switch (aLevelHint)
			{
			case LevelHint.GoodStart:
				componentsInChildren[0].Hint(aLevelHint, aIsWithAnimation);
				break;
			case LevelHint.StartAndFinish:
				componentsInChildren[0].Hint(aLevelHint, aIsWithAnimation);
				componentsInChildren[componentsInChildren.Length - 1].Hint(aLevelHint, aIsWithAnimation, 0.2f);
				break;
			case LevelHint.Expose:
			{
				float num = 0f;
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].Hint(aLevelHint, aIsWithAnimation, num);
					num += 0.2f;
				}
				break;
			}
			}
			return true;
		}
		return false;
	}
}
