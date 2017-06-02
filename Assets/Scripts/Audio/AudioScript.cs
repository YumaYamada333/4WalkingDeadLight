using System.Collections;
using UnityEngine;

public class AudioScript : MonoBehaviour {
    public AudioClip audioClip1;
    public AudioClip audioClip2;
    private AudioSource audioSouce;

	// Use this for initialization
	void Start () {
        audioSouce = gameObject.GetComponent<AudioSource>();
        audioSouce.clip = audioClip1;
    }

    // Update is called once per frame
    void Update () {
		
	}

    AudioSource GetAudioSource()
    {
        return audioSouce;
    }
}
