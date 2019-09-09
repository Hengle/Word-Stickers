using UnityEngine;

public class RoundParticle : MonoBehaviour
{
	public GameObject pod;

	public void SetActive(bool active)
	{
		pod.SetActive(active);
	}
}
