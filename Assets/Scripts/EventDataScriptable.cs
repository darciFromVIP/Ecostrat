using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Event")]
public class EventDataScriptable : ScriptableObject
{
    public Sprite artwork;
    public Sprite eventPicture;
    public AudioClip eventSound;
    [Tooltip("How many seconds until the event appears?")]
    public int time;
    [Tooltip("Does the event repeat after its first appearance? If yes, insert periodic duration in seconds. If not, insert 0.")]
    public int repeatInterval = 0;
    [Tooltip("How many times can the event repeat before permanently disappearing?")]
    public int repeatTimes = 0;
    [Tooltip("How can the player react to this? Create reactions and fill them with Actions, which are created in the File System.")]
    public List<ReactionData> reactions;
    [Tooltip("What happens if player chooses to ignore this event?")]
    public List<Action> ignoreConsequences;
    public string ignoreMessage;
    [TextArea(3, 3)]
    public string eventDescription;
    [Tooltip("Insert 0 for a random location, otherwise this event will spawn on these coordinates.")]
    public Vector2 mapPosition;
    public void ExecuteIgnoreConsequences()
    {
        foreach (var item in ignoreConsequences)
        {
            item.Execute();
        }
    }
}
