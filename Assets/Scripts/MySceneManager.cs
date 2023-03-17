using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MySceneManager : MonoBehaviour
{
    public static MySceneManager instance;
    void Start()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }
    public void Quit()
    {
        Application.Quit();
    }
}
