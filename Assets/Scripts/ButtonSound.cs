using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonSound : MonoBehaviour
{
    public AudioClip sound;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(SoundManager.instance.Click);
    }
}
