using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradesWindow : MonoBehaviour
{
    public static UpgradesWindow instance;
    private void Awake()
    {
        instance = this;
    }
    public void ShowUpgradesWindow()
    {
        gameObject.SetActive(true);
        UpdateButtons();
    }
    public void UpdateButtons()
    {
        foreach (var item in GetComponentsInChildren<UpgradeButton>())
        {
            item.UpdateButton();
        }
    }
}
