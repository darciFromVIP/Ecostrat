using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bubble : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(GameManager.instance.AddMoney);
        button.onClick.AddListener(SoundManager.instance.Click);
        button.onClick.AddListener(DestroySelf);
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
