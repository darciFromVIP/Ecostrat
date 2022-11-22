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
        AddMessage(" Can you save the planet Earth from a Trashcalypse?");
        NextMessage();
    }
    private void Update()
    {
        if (scrollingText.text != "")
        {
            scrollPosition += Time.deltaTime * 40;
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
