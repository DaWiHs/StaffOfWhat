using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    AudioSource source;
    public AudioClip[] clips;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlaySound(int i = -1)
    {
        if (i == -1)
        {
            source.clip = clips[Random.Range(0, clips.Length)];
        } else
        {
            source.clip = clips[i];
        }
        source.Play();
    }
}
