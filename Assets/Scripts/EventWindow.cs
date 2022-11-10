using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EventWindow : MonoBehaviour
{
    public TextMeshProUGUI labelText, descriptionText;
    public Reactions reactions;

    public static EventWindow instance;
    private void Awake()
    {
        instance = this;
    }
    public void UpdateEvent(EventDataScriptable eventData)
    {
        labelText.text = eventData.name;
        descriptionText.text = eventData.eventDescription;
        reactions.SetNewReactions(eventData.reactions);
        Show();
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
