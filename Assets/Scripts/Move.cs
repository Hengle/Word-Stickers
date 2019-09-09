using UnityEngine;

public class Move : MonoBehaviour
{
	public bool moveOnX;

	public bool moveOnY;

	public float amp = 1f;

	private Vector3 originalPos;

	private void Start()
	{
		originalPos = base.transform.position;
	}

	private void FixedUpdate()
	{
		float x = moveOnX ? (originalPos.x + Mathf.Sin(Time.time) * amp) : originalPos.x;
		float y = moveOnY ? (originalPos.y + Mathf.Sin(Time.time) * amp) : originalPos.y;
		base.transform.position = new Vector3(x, y, originalPos.z);
	}
}
