using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    private int money = 500;
    private int followers = 0;
    private float basicTimer = 0;
    private List<GameObject> trashBubbles = new();

    public Texture2D mapSprite;
    public Button bubble;
    public GameObject trashBubble;

    [Header("UI References")]
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI followerText;
    public Canvas mapCanvas;
    public Canvas interactiveCanvas;

    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        Canvas.GetDefaultCanvasMaterial().enableInstancing = true;
        UpdateUI();
        for (int i = 0; i < 10000; i++)
        {
            CreateTrashBubble();
        }
    }
    private void Update()
    {
        basicTimer += Time.deltaTime;
        if (basicTimer >= 2)
        {
            basicTimer = 0;
            CreateBubble();
            CreateTrashBubble();
        }
    }
    private void CreateBubble()
    {
        Button bubbleInstance = Instantiate(bubble, interactiveCanvas.transform);
        bubbleInstance.GetComponent<RectTransform>().anchoredPosition = GetPointOnTerrain();
    }
    private void CreateTrashBubble()
    {
        GameObject bubbleInstance = Instantiate(trashBubble, mapCanvas.transform);
        bubbleInstance.GetComponent<RectTransform>().anchoredPosition = GetPointOnTerrain();
        float random = Random.Range(0.8f, 1.5f);
        bubbleInstance.GetComponent<RectTransform>().localScale = new Vector3(random, random, random);
        trashBubbles.Add(bubbleInstance);
    }
    private Vector2 GetPointOnTerrain()
    {
        Vector2 targetPos = new Vector2(Random.Range(0, mapCanvas.pixelRect.width), Random.Range(0, mapCanvas.pixelRect.height));
        Color color = mapSprite.GetPixel((int)targetPos.x * 4, (int)targetPos.y * 4);
        while (color.r >= 0.202 && color.r <= 0.206 && color.g >= 0.410 && color.g <= 0.414 && color.b >= 0.578 && color.b <= 0.582)    // Sea Color: R 204 G 412 B 480
        {
            targetPos = new Vector2(Random.Range(0, mapCanvas.pixelRect.width), Random.Range(0, mapCanvas.pixelRect.height));
            color = mapSprite.GetPixel((int)targetPos.x * 4, (int)targetPos.y * 4);
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
