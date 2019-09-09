using UnityEngine;

public class MusicManager : ELSingleton<MusicManager>
{
	public AudioClip menu;

	public AudioClip gameNormal;

	public AudioClip gameSpecial;

	public void PlayMenu()
	{
		ELSingleton<AudioManager>.Instance.PlayMusic(menu, loop: true);
	}

	public void PlayGameNormal()
	{
		ELSingleton<AudioManager>.Instance.PlayMusic(gameNormal, loop: true);
	}

	public void PlayGameSpecial()
	{
		ELSingleton<AudioManager>.Instance.PlayMusic(gameSpecial, loop: true);
	}
}
