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
    public void Victory()
    {
        audioSource.PlayOneShot(FindSoundByName("victory"));
    }
    public void Defeat()
    {
        audioSource.PlayOneShot(FindSoundByName("defeat"));
    }
    public void Illegality()
    {
        audioSource.PlayOneShot(FindSoundByName("illegality"));
    }
    public void Income()
    {
        audioSource.PlayOneShot(FindSoundByName("income"));
    }
    public void Penalty()
    {
        audioSource.PlayOneShot(FindSoundByName("penalty"));
    }
    public void Upgrade(int number)
    {
        audioSource.PlayOneShot(FindSoundByName("upgrade" + number.ToString()));
    }
    public void Speed(int number)
    {
        audioSource.PlayOneShot(FindSoundByName(number.ToString() + "xtime"));
    }
    public void PlaySound(AudioClip sound)
    {
        audioSource.PlayOneShot(sound);
    }
}
