using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Event")]
public class EventDataScriptable : ScriptableObject
{
    public Sprite artwork;
    public int time;
    public int repeatTime = 0;
    public List<ReactionData> reactions;
    [TextArea(3, 3)]
    public string eventDescription;
}
