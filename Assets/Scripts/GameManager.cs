using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    private int money = 500;
    private int followers = 0;

    public Button bubble;
    public Image map;
    public GameObject trashBubble;
    private float basicTimer = 0;

    [Header("UI References")]
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI followerText;

    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        UpdateUI();
        for (int i = 0; i < 10000; i++)
        {
            GameObject bubbleInstance = Instantiate(trashBubble, map.transform);
            bubbleInstance.GetComponent<RectTransform>().anchoredPosition = GetPointOnTerrain();
            float random = Random.Range(0.8f, 1.5f);
            bubbleInstance.GetComponent<RectTransform>().localScale = new Vector3(random, random, random);
        }
    }
    private void Update()
    {
        basicTimer += Time.deltaTime;
        if (basicTimer >= 2)
        {
            basicTimer = 0;
            Button bubbleInstance = Instantiate(bubble, map.transform);
            bubbleInstance.GetComponent<RectTransform>().anchoredPosition = GetPointOnTerrain();
        }
    }
    private Vector2 GetPointOnTerrain()
    {
        Vector2 targetPos = new Vector2(Random.Range(0, map.rectTransform.rect.width), Random.Range(0, map.rectTransform.rect.height));
        Color color = map.sprite.texture.GetPixel((int)targetPos.x * 4, (int)targetPos.y * 4);
        while (color.r >= 0.202 && color.r <= 0.206 && color.g >= 0.410 && color.g <= 0.414 && color.b >= 0.578 && color.b <= 0.582)    // Sea Color: R 204 G 412 B 480
        {
            targetPos = new Vector2(Random.Range(0, map.rectTransform.rect.width), Random.Range(0, map.rectTransform.rect.height));
            color = map.sprite.texture.GetPixel((int)targetPos.x * 4, (int)targetPos.y * 4);
        }
        return targetPos;
    }
    public void UpdateUI()
    {
        moneyText.text = money.ToString();
        followerText.text = followers.ToString();
    }
    public void AddMoney()
    {
        money += Random.Range(10, 51);
        UpdateUI();
    }
}
