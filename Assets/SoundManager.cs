using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;

    private AudioSource[] audioSources;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSources = GetComponents<AudioSource>();
    }

    // Update is called once per frame
    public void PlayRandomized(AudioClip audio)
    {
        audioSources[0].pitch = Random.Range(0.9f, 1.1f);
        audioSources[0].PlayOneShot(audio);
    }

    public void PlayPitched(AudioClip audio, int value)
    {
        audioSources[1].pitch = 1f + ((value - 1) * 0.1f);
        audioSources[1].PlayOneShot(audio);
    }

    public void PlayNormal(AudioClip audio)
    {
        audioSources[2].PlayOneShot(audio);
    }
}
