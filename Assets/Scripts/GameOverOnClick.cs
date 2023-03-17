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
        SoundManager.instance.Defeat();
        GameManager.instance.GameOver("Can't outrun justice",
            "You filled your illegal bar to the max and didn't have enough money for the auction. The hand of the law has reached you for this time, but you will definitely avoid it next time!");
    }
}
