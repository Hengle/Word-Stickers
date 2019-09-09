using UnityEngine;

public class HintBar : MonoBehaviour
{
	private const float OFFSET_X = 200f;

	public HintButton[] hintButton;

	private Level level;

	public void Reset()
	{
		level = null;
	}

	public void Set(Level aLevel, float aDelay = 0f)
	{
		level = aLevel;
		Invoke("SetInvoke", aDelay);
	}

	private void SetInvoke()
	{
		if (!base.isActiveAndEnabled || level == null)
		{
			return;
		}
		int num = 0;
		for (int i = 0; i < hintButton.Length; i++)
		{
			if (ELSingleton<HintManager>.Instance.IsHintAvailable(hintButton[i].type, level.number))
			{
				num++;
			}
		}
		float num2 = 0.5f - (float)num / 2f;
		for (int j = 0; j < hintButton.Length; j++)
		{
			if (ELSingleton<HintManager>.Instance.IsHintAvailable(hintButton[j].type, level.number))
			{
				hintButton[j].PositionTargetX = num2 * 200f;
				num2 += 1f;
			}
		}
	}
}
