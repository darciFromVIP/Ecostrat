using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class News : MonoBehaviour
{
    public TextMeshProUGUI textContent;
    public ScrollRect scrollRect;

    public static News instance;
    private void Awake()
    {
        instance = this;
    }

    public void AddMessage(string message)
    {
        SoundManager.instance.RadioTalk();
        textContent.text += "\n\n[" + GameManager.instance.GetTimeStamp() + "] " + message;
        Canvas.ForceUpdateCanvases();

        GetComponentInChildren<VerticalLayoutGroup>().CalculateLayoutInputVertical();
        GetComponentInChildren<ContentSizeFitter>().SetLayoutVertical();

        scrollRect.verticalNormalizedPosition = 0;
    }
}
