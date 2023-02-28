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
        GameManager.instance.GameOver("Can't outrun justice", 
            "You've reached 100 illegality points, which means you've been found guilty in court and your buyout bid is set at $100,000.");
    }
}
