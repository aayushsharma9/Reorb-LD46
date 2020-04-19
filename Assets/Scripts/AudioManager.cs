using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public Sound[] sounds;
	Hashtable soundMap = new Hashtable();
	public static AudioManager instance;

	void Awake()
	{
		DontDestroyOnLoad(gameObject);

		if (instance == null) instance = this;
		else
		{
			Destroy(gameObject);
			return;
		}

		foreach (Sound s in sounds)
		{
			s.audioSource = gameObject.AddComponent<AudioSource>();
			s.audioSource.clip = s.clip;
			s.name = s.clip.name;
			s.audioSource.volume = s.volume;
			s.audioSource.pitch = s.pitch;
			s.audioSource.playOnAwake = s.playOnAwake;
			s.audioSource.loop = s.loop;
			soundMap.Add(s.name, s);
		}

		Play("Music");
	}

	public void Play(string name)
	{
		// Sound s = Array.Find(sounds, sound => sound.name == name);
		Debug.Log(name);
		Sound s = (Sound) soundMap[name];
		s.audioSource.Play();
		Debug.Log("CLIP: " + s.audioSource.clip);
	}
}