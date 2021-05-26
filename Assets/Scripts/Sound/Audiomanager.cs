using UnityEngine.Audio;
using UnityEngine;
using System;
using System.Collections;

// this is my standard audio manager, which I should revise a bit.
public class Audiomanager : MonoBehaviour {

	public Sound[] sounds;

	public static Audiomanager Instance;

	public static float fadetime = 0.5f;
    
	void Awake () {
		// instanciate audio source for each sound in array
		foreach (Sound s in sounds) {
			s.source = gameObject.AddComponent<AudioSource> ();
			s.source.clip = s.clip;
			s.source.loop = s.loop;
		}

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

	/// <summary>Play sound by name.</summary>
	/// <param name="name">Name of sound object to be played.</param>
	public void Play(string name){
		Sound s = Array.Find (sounds, sound => sound.name == name);
		if (s == null) {
			Debug.Log ("Sound " + s + " was not found");
			return;
		}


		// fade in if sound is music
		if (s.music) {
			StartCoroutine (FadeIn (s.source, s.volume));
		} else {
			s.source.Play ();
		}
	}

	/// <summary>Stop sound by name.</summary>
	/// <param name="name">Name of sound object to be stopped.</param>
	public void Stop(string name){
		Sound s = Array.Find (sounds, sound => sound.name == name);
		if (s == null) {
			Debug.Log ("Sound " + s + " was not found");
			return;
		}

		// fade out if sound is music
		if (s.music) {
			StartCoroutine (FadeOut (s.source));
		} else {
			s.source.Stop ();
		}
	}

	public static IEnumerator FadeIn (AudioSource audioSourceStart, float maxVolume) {
		
		audioSourceStart.volume = 0f;
		audioSourceStart.Play ();
		float startVolume = 0.1f;

		while (audioSourceStart.volume < maxVolume) {
			audioSourceStart.volume += startVolume * Time.deltaTime / fadetime;
			yield return null;
		}
	}

	public static IEnumerator FadeOut (AudioSource audioSource) {
		float startVolume = audioSource.volume;

		while (audioSource.volume > 0.1f) {
			audioSource.volume -= startVolume * Time.deltaTime / fadetime;

			yield return null;
		}

		audioSource.volume = 0;
		audioSource.Stop ();
	}

	/// <summary>Toggles mut on all sources.</summary>
	public bool ToggleMuteAll()
    {
        AudioSource[] allAudios = this.GetComponents<AudioSource>();
        foreach (AudioSource s in allAudios)
        {
            s.mute = !s.mute;
        }

        return allAudios[0].mute;
    }

	/// <summary>Check if muted.</summary>
	private bool IsMuted()
    {
        bool muted = this.GetComponent<AudioSource>().mute;
        return muted;
    }


}
