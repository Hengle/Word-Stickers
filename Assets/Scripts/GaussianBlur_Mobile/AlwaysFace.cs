using UnityEngine;

namespace GaussianBlur_Mobile
{
	public class AlwaysFace : MonoBehaviour
	{
		public GameObject Target;

		public float Speed;

		public bool JustOnStart;

		private void Start()
		{
			Quaternion rotation = Quaternion.LookRotation(Target.transform.position - base.transform.position);
			base.gameObject.transform.rotation = rotation;
		}

		private void FixedUpdate()
		{
			if (!JustOnStart && Target != null)
			{
				Quaternion b = Quaternion.LookRotation(Target.transform.position - base.transform.position);
				base.gameObject.transform.rotation = Quaternion.Lerp(base.gameObject.transform.rotation, b, Speed * Time.deltaTime);
			}
		}
	}
}
