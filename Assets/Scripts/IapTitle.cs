using UnityEngine;
using UnityEngine.UI;

public class IapTitle : MonoBehaviour
{
	public Text title;

	private bool isVisible = true;

	public string Title
	{
		set
		{
			title.text = value;
		}
	}

	public void CheckVisible()
	{
		Vector3 vector = Camera.main.WorldToViewportPoint(base.gameObject.transform.position);
		bool flag = vector.y >= -0.1f && vector.y <= 1.1f;
		if (flag != isVisible)
		{
			base.gameObject.SetActive(flag);
		}
		isVisible = flag;
	}
}
