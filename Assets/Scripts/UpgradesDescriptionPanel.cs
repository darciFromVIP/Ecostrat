using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradesDescriptionPanel : MonoBehaviour
{
    public TextMeshProUGUI nameText, priceText, descriptionText, benefitsText;

    public static UpgradesDescriptionPanel instance;
    private void Awake()
    {
        instance = this;
    }
    public void UpdateDescription(UpgradeInfo info)
    {
        nameText.text = info.name;
        priceText.text = info.price * GameManager.instance.priceModifier + "<sprite=1>";
        descriptionText.text = info.description;
        benefitsText.text = info.benefits;
    }
}
