using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class News : MonoBehaviour
{
    public TextMeshProUGUI scrollingText;
    private float width;
    private Vector2 startingPosition;
    private float scrollPosition = 0;
    private Queue<string> messages = new();

    public static News instance;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        startingPosition = scrollingText.rectTransform.anchoredPosition;
        AddMessage("Welcome to Ecostrat!");
        AddMessage("You have one year left to save our planet. There is not a lot of time, so you need to act as quickly as possible!");
        NextMessage();
    }
    private void Update()
    {
        if (scrollingText.text != "" && !GameManager.instance.paused)
        {
            scrollPosition += Time.deltaTime * 50 * GameManager.instance.speed;
            scrollingText.rectTransform.anchoredPosition = new Vector3(-scrollPosition, startingPosition.y);
            if (scrollPosition > width)
            {
                scrollPosition = 0;
                scrollingText.rectTransform.anchoredPosition = new Vector3(-scrollPosition, startingPosition.y);
                NextMessage();
            }
        }
    }

    public void AddMessage(string message)
    {
        SoundManager.instance.RadioTalk();
        messages.Enqueue(message);
        if (scrollingText.text == "")
            NextMessage();
    }
    private void NextMessage()
    {
        if (messages.Count > 0)
        {
            scrollingText.text = messages.Dequeue();
            width = scrollingText.preferredWidth + GetComponent<RectTransform>().rect.width;
        }
        else
            scrollingText.text = "";
    }
}
