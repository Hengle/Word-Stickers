using UnityEngine;

public class AddTorqueOnStart : MonoBehaviour
{
	public float RandonAmmount;

	private void Start()
	{
		base.gameObject.GetComponent<Rigidbody>().AddTorque(UnityEngine.Random.Range(0f - RandonAmmount, RandonAmmount), UnityEngine.Random.Range(0f - RandonAmmount, RandonAmmount), UnityEngine.Random.Range(0f - RandonAmmount, RandonAmmount));
	}
}
