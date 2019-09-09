using UnityEngine;

public class ELSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T instance;

	public static T Instance
	{
		get
		{
			if ((Object)instance == (Object)null)
			{
				instance = (T)UnityEngine.Object.FindObjectOfType(typeof(T));
				if ((Object)instance == (Object)null && (Object)instance == (Object)null)
				{
					UnityEngine.Debug.LogWarning("ELSingleton: an instance of " + typeof(T) + " is needed in the scene, but there is none.");
				}
			}
			return instance;
		}
	}

	public static T CreateInstanceOfType(string[] windows, Transform parent)
	{
		for (int i = 0; i < windows.Length; i++)
		{
			if (!windows[i].EndsWith(typeof(T).Name))
			{
				continue;
			}
			T component = (UnityEngine.Object.Instantiate(Resources.Load("Prefabs/" + windows[i], typeof(GameObject)), parent.position, parent.rotation, parent) as GameObject).GetComponent<T>();
			int num = component.transform.GetSiblingIndex();
			int num2 = GetWindowIndex(component.gameObject, windows);
			while (num > 0)
			{
				num--;
				if (GetWindowIndex(parent.GetChild(num).gameObject, windows) > num2)
				{
					num2 = num;
				}
			}
			component.transform.SetSiblingIndex(num2);
			return component;
		}
		return null;
	}

	public static int GetWindowIndex(GameObject type, string[] windows)
	{
		for (int i = 0; i < windows.Length; i++)
		{
			if (type.name.StartsWith(windows[i]))
			{
				return i;
			}
		}
		return int.MaxValue;
	}
}
