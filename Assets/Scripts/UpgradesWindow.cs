using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradesWindow : MonoBehaviour
{
    public static UpgradesWindow instance;
    private void Start()
    {
        instance = this;
        gameObject.SetActive(false);
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
