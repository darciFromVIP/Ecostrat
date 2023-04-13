using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioSoundManager : MonoBehaviour
{
    private AudioSource audioSource;
    public static RadioSoundManager instance;
    public List<AudioClip> radioSounds;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        instance = this;
    }
    public void PlayRadioSound()
    {
        if (!audioSource.isPlaying)
        {
            int random = Random.Range(0, radioSounds.Count);
            audioSource.PlayOneShot(radioSounds[random]);
        }
    }
    public void StopPlaying()
    {
        audioSource.Stop();
    }
}
