using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class SoundManager : MonoBehaviour
{
    public List<AudioClip> sounds;
    private AudioSource audioSource;

    public static SoundManager instance;

    private void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }
    public AudioClip FindSoundByName(string name)
    {
        return sounds.Find((x) => x.name == name);
    }
    public void EventOpen()
    {
        audioSource.PlayOneShot(FindSoundByName("eventopen"));
    }
    public void Click()
    {
        audioSource.PlayOneShot(FindSoundByName("click"));
    }
    public void RadioTalk()
    {
        audioSource.PlayOneShot(FindSoundByName("radiotalk"));
    }
    public void PlaySound(AudioClip sound)
    {
        audioSource.PlayOneShot(sound);
    }
}
