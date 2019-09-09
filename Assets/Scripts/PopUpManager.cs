using UnityEngine;

public class PopUpManager : ELSingleton<PopUpManager>
{
	public MonoBehaviour[] popUps;

	public bool IsActiveAndEnabled()
	{
		MonoBehaviour[] array = popUps;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].isActiveAndEnabled)
			{
				return true;
			}
		}
		return false;
	}
}
