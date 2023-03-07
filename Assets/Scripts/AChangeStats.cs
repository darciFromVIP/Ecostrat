using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/Stat Change")]
public class AChangeStats : Action
{
    public PlayerStat stat;
    public float modifier;
    public override void Execute()
    {
        if (modifier < 0 && stat == PlayerStat.Money)
            GameManager.instance.ChangeStats(stat, modifier * GameManager.instance.priceModifier);
        else
            GameManager.instance.ChangeStats(stat, modifier);
    }

    public override void SecondaryExecute()
    {
        GameManager.instance.ChangeStats(stat, -modifier);
    }

    public override bool TestExecute()
    {
        return GameManager.instance.TestChangeStats(stat, modifier);
    }
}
