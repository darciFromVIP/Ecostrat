using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ReactionData 
{
    [TextArea(3, 3)]
    public string description;
    public string additionalDescription;
    public List<Action> actions = new();
    [TextArea(3, 3)]
    public string consequenceMessage;

    public void ExecuteActions()
    {
        foreach (var item in actions)
        {
            item.Execute();
        }
        News.instance.AddMessage(consequenceMessage);
    }
    public void ExecuteSecondaryActions()
    {
        foreach (var item in actions)
        {
            item.SecondaryExecute();
        }
    }
    public bool TestExecute()
    {
        bool valid = true;
        foreach (var item in actions)
        {
            if (!item.TestExecute())
            {
                valid = false;
                break;
            }
        }
        return valid;
    }
}
