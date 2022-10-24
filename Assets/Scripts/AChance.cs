using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/Chance")]
public class AChance : Action
{
    public List<int> chances;
    public List<Action> actions;
    public override void Execute()
    {
        int random = Random.Range(1, 101);
        int lowerInterval = 1;
        for (int i = 0; i < chances.Count; i++)
        {
            if (random >= lowerInterval && random < lowerInterval + chances[i])
            {
                actions[i].Execute();
                break;
            }
            lowerInterval += chances[i];
        }
    }

    public override void SecondaryExecute()
    {
        throw new System.NotImplementedException();
    }

    public override bool TestExecute()
    {
        bool valid = true;
        foreach (var item in actions)
        {
            if (!item.TestExecute())
                valid = false;
        }
        return valid;
    }
}
