using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Event : MonoBehaviour
{
    private Button btn;
    public TextMeshProUGUI labelText, descriptionText;
    public GameObject eventWindow;
    public Reactions reactions;

    private void Awake()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(ToggleReactions);
    }
    public void UpdateEvent(EventDataScriptable eventData)
    {
        labelText.text = eventData.name;
        descriptionText.text = eventData.eventDescription;
        reactions.SetNewReactions(eventData.reactions);
        eventWindow.SetActive(false);
    }
    private void ToggleReactions()
    {
        eventWindow.SetActive(!eventWindow.activeSelf);
    }
    public void Destroy()
    {
        Destroy(gameObject);
    }
}
