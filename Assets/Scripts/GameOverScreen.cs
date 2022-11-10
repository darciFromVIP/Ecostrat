using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverScreen : MonoBehaviour
{
    public TextMeshProUGUI label, description;
    public void UpdateTexts(string label, string description)
    {
        gameObject.SetActive(true);
        this.label.text = label;
        this.description.text = description;
    }
}
