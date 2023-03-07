using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Reaction : MonoBehaviour
{
    public TextMeshProUGUI description;
    public TextMeshProUGUI price;
    public TextMeshProUGUI hint;

    private Button btn;
    private void Awake()
    {
        btn = GetComponent<Button>();
    }
    public void UpdateReaction(ReactionData data)
    {
        btn.interactable = true;
        int priceValue = (int)(data.GetPrice() * GameManager.instance.priceModifier);
        if (priceValue > 0)
            price.text = "<sprite=1>" + priceValue.ToString();
        else
            price.text = "<sprite=1>+" + (priceValue * -1).ToString();
        btn.onClick.AddListener(data.ExecuteActions);
        btn.onClick.AddListener(EventWindow.instance.Hide);
        description.text = data.description;
        if (GameManager.instance.hints > 0)
            hint.text = data.additionalDescription;
        if (!data.TestExecute())
            btn.interactable = false;
    }
}
