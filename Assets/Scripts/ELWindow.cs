using UnityEngine;

public class ELWindow<T> : ELSingleton<T> where T : MonoBehaviour
{
	private bool isInitialized;

	public void Start()
	{
		if (!isInitialized)
		{
			HideWindow();
			isInitialized = true;
		}
	}

	private void ButtonPop()
	{
		if (!base.isActiveAndEnabled)
		{
			return;
		}
		CommonButton[] componentsInChildren = base.gameObject.GetComponentsInChildren<CommonButton>();
		int num = UnityEngine.Random.Range(0, componentsInChildren.Length);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			int num2 = (num + i) % componentsInChildren.Length;
			if (componentsInChildren[num2].isActiveAndEnabled && componentsInChildren[num2].IsEnabled)
			{
				componentsInChildren[num2].Pop();
				break;
			}
		}
		Invoke("ButtonPop", UnityEngine.Random.Range(3f, 5f));
	}

	public void InitWindow()
	{
		base.transform.localPosition = new Vector2(0f, 0f);
		RectTransform component = base.transform.GetComponent<RectTransform>();
		RectTransform component2 = ELSingleton<ELCanvas>.Instance.GetComponent<RectTransform>();
		component.sizeDelta = new Vector2(component2.rect.width, component2.rect.height);
	}

	public void ShowWindow()
	{
		isInitialized = true;
		base.transform.localPosition = new Vector2(0f, 0f);
		RectTransform component = base.transform.GetComponent<RectTransform>();
		RectTransform component2 = ELSingleton<ELCanvas>.Instance.GetComponent<RectTransform>();
		component.sizeDelta = new Vector2(component2.rect.width, component2.rect.height);
		ELSingleton<T>.Instance.gameObject.SetActive(value: true);
		Invoke("ButtonPop", UnityEngine.Random.Range(3f, 5f));
		if (ELSingleton<AnalyticsManager>.Instance != null)
		{
			ELSingleton<AnalyticsManager>.Instance.ScreenVisit(base.name);
		}
	}

	public void HideWindow()
	{
		ELSingleton<T>.Instance.gameObject.SetActive(value: false);
	}
}
