using UnityEngine.Audio;
using UnityEngine;

// pattern soundclip
[System.Serializable]
public class Sound {

	public AudioClip clip;

	public string name;

	[Range(0f, 1f)]
	public float volume;


	public bool loop;

	public bool music;

	[HideInInspector]
	public AudioSource source;

}
