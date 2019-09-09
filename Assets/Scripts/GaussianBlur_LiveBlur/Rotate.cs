using UnityEngine;

namespace GaussianBlur_LiveBlur
{
	public class Rotate : MonoBehaviour
	{
		public float speed;

		private float yRotataion;

		private void FixedUpdate()
		{
			yRotataion += speed;
			base.gameObject.transform.rotation = Quaternion.Euler(0f, yRotataion, 0f);
		}
	}
}
