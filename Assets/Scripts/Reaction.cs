using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Reaction : MonoBehaviour
{
    public TextMeshProUGUI description;

    private Button btn;
    private void Awake()
    {
        btn = GetComponent<Button>();
    }
    public void UpdateReaction(ReactionData data)
    {
        btn.interactable = true;
        btn.onClick.AddListener(data.ExecuteActions);
        btn.onClick.AddListener(EventWindow.instance.Hide);
        if (GameManager.instance.hints > 0)
            description.text = data.description + "" + data.additionalDescription;
        else
            description.text = data.description;
        if (!data.TestExecute())
            btn.interactable = false;
    }
}
