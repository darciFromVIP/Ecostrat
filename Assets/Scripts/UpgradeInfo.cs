using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UpgradeType
{
    Negotiation, SocialSites, Riots, SocialEvents, OceanCleansing, Hacking, Bribe, Blackmail, Vandalism, Landfills
}

[CreateAssetMenu(menuName = "Upgrade Info")]
public class UpgradeInfo : ScriptableObject
{
    public Sprite artwork;
    public UpgradeType upgradeType;
    public int price;
    [TextArea(3, 3)]
    public string description;
    [TextArea(3, 3)]
    public string benefits;
    public List<Action> actions = new();
}
