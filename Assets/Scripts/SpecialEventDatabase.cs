using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Special Event Database")]
[System.Serializable]
public class SpecialEventDatabase : ScriptableObject
{
    public List<EventDataScriptable> events = new();

#if UNITY_EDITOR
    [ContextMenu("Load Events Into Database")]
    public void LoadEventsIntoDatabase()
    {
        List<EventDataScriptable> tempEvents = new();
        tempEvents.Clear();
        string[] assetNames = AssetDatabase.FindAssets("", new[] { "Assets/Game Data/Special Events" });
        foreach (string SOName in assetNames)
        {
            var SOpath = AssetDatabase.GUIDToAssetPath(SOName);
            var character = AssetDatabase.LoadAssetAtPath<EventDataScriptable>(SOpath);
            tempEvents.Add(character);
        }
        events.Clear();
        foreach (var item in tempEvents)
        {
            events.Add(item);
        }
    }
#endif
    public EventDataScriptable GetRandomEvent()
    {
        return events[Random.Range(0, events.Count)]; 
    }
}
