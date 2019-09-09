using UnityEngine;

public class RateUsInside : ELSingleton<RateUsInside>
{
	public ParticleSystem particle;

	public void StartParticle()
	{
		particle.Emit(1);
		particle.Play();
	}
}
