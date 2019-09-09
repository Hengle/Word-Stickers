using UnityEngine;
using UnityEngine.UI;

public class TimeMeter : MonoBehaviour
{
	public GameObject markerPod;

	public Transform markerStart;

	public Transform markerEnd;

	public Text timeText;

	public bool IsEnabled
	{
		private get;
		set;
	}

	public float TimeOrigin
	{
		get;
		private set;
	}

	public float TimeCurrent
	{
		get;
		private set;
	}

	public void Reset()
	{
		if (base.isActiveAndEnabled)
		{
			markerPod.transform.position = markerStart.position;
			timeText.text = string.Empty;
			IsEnabled = false;
			TimeOrigin = 0f;
			TimeCurrent = 0f;
			Animator component = base.gameObject.GetComponent<Animator>();
			component.ResetTrigger("Show");
			component.ResetTrigger("Hide");
			component.Play("Init");
		}
	}

	public void Set(float aTime)
	{
		IsEnabled = false;
		TimeOrigin = aTime;
		TimeCurrent = TimeOrigin;
		Setup();
	}

	public void Setup()
	{
		if (IsEnabled)
		{
			TimeCurrent -= Time.deltaTime;
			if (TimeCurrent < 0f)
			{
				TimeCurrent = 0f;
			}
		}
		if (TimeOrigin > 0f)
		{
			float t = 1f - TimeCurrent / TimeOrigin;
			markerPod.transform.position = Vector3.Lerp(markerStart.position, markerEnd.position, t);
		}
		timeText.text = Mathf.Ceil(TimeCurrent).ToString();
	}

	public void Show(float aDelay = 0f)
	{
		Invoke("ShowInvoke", aDelay);
	}

	private void ShowInvoke()
	{
		if (base.isActiveAndEnabled)
		{
			base.gameObject.GetComponent<Animator>().SetTrigger("Show");
		}
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
}
