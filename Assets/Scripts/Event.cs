using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Event : MonoBehaviour
{
    private Button btn;
    private Image image;
    private EventDataScriptable eventData;
    public bool isHinted = false;
    [SerializeField] private Slider slider;
    private float timer = 60;
    private void Awake()
    {
        image = GetComponent<Image>();
        btn = GetComponent<Button>();
        btn.onClick.AddListener(ShowEventWindow);
        btn.onClick.AddListener(SoundManager.instance.EventOpen);
        btn.onClick.AddListener(PlayEventSound);
    }
    private void Update()
    {
        if (GameManager.instance.paused || eventData.name == "Amogus")
            return;
        timer -= Time.deltaTime * GameManager.instance.speed;
        slider.value = timer;
        if (timer <= 0)
        {
            SoundManager.instance.Penalty();
            eventData.ExecuteIgnoreConsequences();
            News.instance.AddMessage(eventData.ignoreMessage);
            Destroy();
        }
    }
    public void UpdateEvent(EventDataScriptable eventData)
    {
        this.eventData = eventData;
        image.sprite = eventData.artwork;
    }
    private void ShowEventWindow()
    {
        EventWindow.instance.UpdateEvent(eventData, this);
    }
    public void Destroy()
    {
        Destroy(gameObject);
    }
    private void PlayEventSound()
    {
        SoundManager.instance.PlaySound(eventData.eventSound);
    }
}
