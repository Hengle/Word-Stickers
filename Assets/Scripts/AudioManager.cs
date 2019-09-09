using UnityEngine;

public class AudioManager : ELSingleton<AudioManager>
{
	public AudioSource sfxSource;

	public AudioSource musicSource;

	private bool _sfxMute;

	private float _sfxVolume = 0.5f;

	private bool _musicMute;

	private float _musicVolume = 0.5f;

	public float sfxVolume
	{
		get
		{
			return _sfxVolume;
		}
		set
		{
			_sfxVolume = value;
			_sfxMute = (value == 0f);
			if (_sfxMute)
			{
				StopSfx();
			}
		}
	}

	public bool sfxMute
	{
		get
		{
			return _sfxMute;
		}
		set
		{
			_sfxMute = value;
			if (_sfxMute)
			{
				StopSfx();
			}
		}
	}

	public float musicVolume
	{
		get
		{
			return _musicVolume;
		}
		set
		{
			_musicVolume = value;
			musicSource.volume = value;
			_musicMute = (value == 0f);
			if (_musicMute)
			{
				StopMusic();
			}
		}
	}

	public bool musicMute
	{
		get
		{
			return _musicMute;
		}
		set
		{
			_musicMute = value;
			if (_musicMute)
			{
				StopMusic();
			}
		}
	}

	public void PlaySfx(AudioClip clip)
	{
		if (!_sfxMute)
		{
			sfxSource.PlayOneShot(clip, sfxVolume);
		}
	}

	public void StopSfx()
	{
		sfxSource.Stop();
	}

	public void PlayMusic(AudioClip clip, bool loop)
	{
		if (!_musicMute)
		{
			musicSource.loop = loop;
			musicSource.volume = musicVolume;
			musicSource.clip = clip;
			musicSource.Play();
		}
	}

	public void StopMusic()
	{
		musicSource.Stop();
	}

	public void PauseMusic()
	{
		if (musicSource.isPlaying)
		{
			musicSource.Pause();
		}
	}

	public void ResumeMusic()
	{
		if (!_musicMute && !musicSource.isPlaying)
		{
			musicSource.Play();
		}
	}
}
