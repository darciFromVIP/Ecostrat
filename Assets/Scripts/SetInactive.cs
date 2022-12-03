using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SetInactive : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(TakeMoney);
    }
    private void Start()
    {
        if (!GameManager.instance.TestChangeStats(PlayerStat.Money, -50000))
            GetComponent<Button>().interactable = false;
    }
    private void TakeMoney()
    {
        GameManager.instance.ChangeStats(PlayerStat.Illegality, -50);
        GameManager.instance.ChangeStats(PlayerStat.Money, -50000);
    }
}
