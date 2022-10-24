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
    private void SetButtonInteractability()
    {
        btn.interactable = false;
    }
    public void UpdateReaction(ReactionData data)
    {
        btn.interactable = true;
        btn.onClick.AddListener(data.ExecuteActions);
        btn.onClick.AddListener(GetComponentInParent<Event>().Destroy);
        description.text = data.description;
        if (!data.TestExecute())
            btn.interactable = false;
    }
}
