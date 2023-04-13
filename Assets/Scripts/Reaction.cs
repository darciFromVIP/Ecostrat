using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Reaction : MonoBehaviour
{
    public TextMeshProUGUI label;
    public TextMeshProUGUI description;
    public TextMeshProUGUI price;
    public TextMeshProUGUI hint;

    private Button btn;
    private void Awake()
    {
        btn = GetComponent<Button>();
    }
    public void UpdateReaction(ReactionData data, bool hinted)
    {
        btn.interactable = true;
        int priceValue = data.GetPrice();
        if (priceValue > 0)
        {
            priceValue = (int)(priceValue * GameManager.instance.priceModifier);
            price.text = "<sprite=1>" + priceValue.ToString();
        }
        else
            price.text = "<sprite=1>+" + (priceValue * -1).ToString();
        btn.onClick.AddListener(data.ExecuteActions);
        btn.onClick.AddListener(EventWindow.instance.Hide);
        label.text = data.label;
        description.text = data.description;
        if (hinted)
            hint.text = data.additionalDescription;
        if (!data.TestExecute())
            btn.interactable = false;
    }
}
