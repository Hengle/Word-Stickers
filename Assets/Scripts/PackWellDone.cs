using UnityEngine;

public class PackWellDone : MonoBehaviour
{
	public ParticleSystem particle;

	public void StartParticle()
	{
		particle.Emit(1);
		particle.Play();
	}
}
