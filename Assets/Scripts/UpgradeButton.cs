using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UpgradeButton : MonoBehaviour
{
    private Button btn;
    public List<UpgradeInfo> upgradeInfos = new();
    public UpgradeInfo currentUpgradeInfo;
    private int currentIndex = 0;
    private void Awake()
    {
        btn = GetComponent<Button>();
        currentUpgradeInfo = upgradeInfos[currentIndex];
        currentIndex++;
    }
    public void UpdateButton()
    {
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(UpgradeEvent);
        if (GameManager.instance.money < currentUpgradeInfo.price * GameManager.instance.priceModifier)
            btn.interactable = false;
        else
            btn.interactable = true;
        if (name == "OceanCleansing" && !GameManager.instance.LegalUltimatePerkUnlocked())
            btn.interactable = false;
        if (name == "Vandalism" && !GameManager.instance.IllegalUltimatePerkUnlocked())
            btn.interactable = false;
        GetComponent<Image>().sprite = currentUpgradeInfo.artwork;
    }
    public void UpdateDescription()
    {
        UpgradesDescriptionPanel.instance.UpdateDescription(currentUpgradeInfo);
    }
    private void UpgradeEvent()
    {
        GameManager.instance.UpgradePerk(currentUpgradeInfo);
        currentUpgradeInfo = upgradeInfos[currentIndex];
        if (currentIndex + 1 == upgradeInfos.Count)
            btn.enabled = false;
        else
            currentIndex++;
        UpgradesWindow.instance.UpdateButtons();
    }
}
