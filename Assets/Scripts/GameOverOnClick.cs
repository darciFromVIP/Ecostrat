using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameOverOnClick : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(GameOver);
    }
    private void GameOver()
    {
        GameManager.instance.GameOver("Can't outrun justice", "All of your hope is lost, and humanity's as well. Who's gonna save the world now?");
    }
}
