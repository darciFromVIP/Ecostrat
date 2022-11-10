using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Event : MonoBehaviour
{
    private Button btn;
    private Image image;
    private EventDataScriptable eventData;
    private void Awake()
    {
        image = GetComponent<Image>();
        btn = GetComponent<Button>();
        btn.onClick.AddListener(ShowEventWindow);
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
}
