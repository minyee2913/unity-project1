using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip[] sounds = { };

    AudioSource Audio;
    void Start()
    {
        Audio = GetComponent<AudioSource>();
    }

public void Play()
    {
        StartCoroutine(OnPlay());
    }

    public IEnumerator OnPlay()
    {
        if (sounds.Length > 1) {
            Audio.clip = sounds[0];
            Audio.loop = false;
            Audio.Play();

            while (true)
            {
                yield return null;
                if (!Audio.isPlaying)
                {
                    Audio.clip = sounds[1];
                    Audio.Play();
                    Audio.loop = true;

                    break;
                }
            }
        } else
        {
            Audio.clip = sounds[0];
            Audio.loop = true;
            Audio.Play();
        }
    }
}
