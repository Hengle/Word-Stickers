using UnityEngine;

public class CommonRayPod : MonoBehaviour
{
	public GameObject[] ray;

	private void Update()
	{
		for (int i = 0; i < ray.Length; i++)
		{
			ray[i].transform.Rotate(new Vector3(0f, 0f, (float)((i % 2 == 0) ? 1 : (-1)) * 10f * Time.deltaTime));
		}
	}
}
