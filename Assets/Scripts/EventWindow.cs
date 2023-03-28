using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EventWindow : MonoBehaviour
{
    public TextMeshProUGUI labelText, descriptionText;
    public Reactions reactions;
    private Event currentEvent;
    public Button ignoreButton;
    public Image eventPicture;

    public static EventWindow instance;
    private void Awake()
    {
        instance = this;
        Hide();
    }
    public void UpdateEvent(EventDataScriptable eventData, Event currentEvent = null)
    {
        Show();
        eventPicture.sprite = eventData.eventPicture;
        this.currentEvent = currentEvent;
        if (GameManager.instance.hints > 0)
        {
            currentEvent.isHinted = true;
            GameManager.instance.ChangeStats(PlayerStat.Hint, -1);
        }
        labelText.text = eventData.name;
        descriptionText.text = eventData.eventDescription;
        reactions.SetNewReactions(eventData.reactions, currentEvent.isHinted);
        reactions.AddListenerToReactions(EventResolved);
        ignoreButton.onClick.RemoveAllListeners();
        ignoreButton.onClick.AddListener(Hide);
    }
    public void Show()
    {
        gameObject.SetActive(true);
        GameManager.instance.PauseGameToggle(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
        GameManager.instance.PauseGameToggle(false);
    }
    public void EventResolved()
    {
        if (currentEvent)
            currentEvent.Destroy();
        Hide();
    }
}
