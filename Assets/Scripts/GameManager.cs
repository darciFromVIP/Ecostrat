using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int money = 500;
    private int followers = 0;
    private int illegality = 0;
    [SerializeField] private int trash = 0;
    private float gameTimer = 3600;
    private float bubbleTimer = 0;
    private float trashTimer = 0;
    private float illegalityTimer = 0;
    private int trashIncrementAmount = 10;
    private int trashIncrementInterval = 3;
    private List<GameObject> trashBubbles = new();
    private bool paused = false;

    private int negotiationLevel = 0;
    private int socialSitesLevel = 0;
    private int riotsLevel = 0;
    private int socialEventsLevel = 0;
    private int oceanCleansingLevel = 0;
    private int hackingLevel = 0;
    private int bribeLevel = 0;
    private int blackmailLevel = 0;
    private int vandalismLevel = 0;

    public Texture2D mapSprite;
    public Button bubblePrefab;
    public GameObject trashBubblePrefab;
    public Event eventPrefab;
    public EventDatabase eventDatabase;

    [Header("UI References")]
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI followerText;
    public Slider gameTimerSlider;
    public Slider illegalitySlider;
    public Canvas mapCanvas;
    public Canvas interactiveCanvas;
    public GameOverScreen gameoverScreen;

    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        Canvas.GetDefaultCanvasMaterial().enableInstancing = true;
        UpdateUI();
        trash = 10000;
        for (int i = 0; i < trash; i++)
        {
            CreateTrashBubble();
        }
        foreach (var item in eventDatabase.events)
        {
            StartCoroutine(StartEventTimer(item));
        }
    }
    private void Update()
    {
        if (paused)
            return;
        gameTimer -= Time.deltaTime;
        gameTimerSlider.value = gameTimer;
        if (gameTimer <= 0)
            GameOver("Not enough time...", "Unfortunately, your operations have been too slow and weren't sufficient to save the planet in time.");
        bubbleTimer += Time.deltaTime;
        if (bubbleTimer >= 2)
        {
            bubbleTimer = 0;
            CreateBubble();
        }
        trashTimer += Time.deltaTime;
        if (trashTimer >= trashIncrementInterval)
        {
            trashTimer = 0;
            trash += trashIncrementAmount;
            for (int i = 0; i < trashIncrementAmount; i++)
            {
                CreateTrashBubble();
            }
        }
        illegalityTimer += Time.deltaTime;
        if (illegalityTimer >= 60)
        {
            illegality -= 5;
            if (illegality < 0)
                illegality = 0;
            illegalityTimer = 0;
        }
    }
    private void CreateBubble()
    {
        Button bubbleInstance = Instantiate(bubblePrefab, interactiveCanvas.transform);
        bubbleInstance.GetComponent<RectTransform>().anchoredPosition = GetPointOnTerrain();
    }
    private void CreateTrashBubble()
    {
        GameObject bubbleInstance = Instantiate(trashBubblePrefab, mapCanvas.transform);
        bubbleInstance.GetComponent<RectTransform>().anchoredPosition = GetPointOnTerrain();
        float random = Random.Range(0.8f, 1.5f);
        bubbleInstance.GetComponent<RectTransform>().localScale = new Vector3(random, random, random);
        trashBubbles.Add(bubbleInstance);
    }
    private void RemoveTrashBubble()
    {
        trashBubbles.Remove(trashBubbles[trashBubbles.Count - 1]);
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
    private Vector2 GetPointOnWater()
    {
        Vector2 targetPos = new Vector2(Random.Range(0, mapCanvas.pixelRect.width), Random.Range(0, mapCanvas.pixelRect.height));
        Color color = mapSprite.GetPixel((int)targetPos.x * 4, (int)targetPos.y * 4);
        while (!(color.r >= 0.202 && color.r <= 0.206 && color.g >= 0.410 && color.g <= 0.414 && color.b >= 0.578 && color.b <= 0.582))
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
        illegalitySlider.value = illegality;
    }
    public void AddMoney()
    {
        money += Random.Range(10, 51);
        UpdateUI();
    }
    public void ChangeStats(PlayerStat stat, int modifier)
    {
        switch (stat)
        {
            case PlayerStat.Followers:
                followers += modifier;
                break;
            case PlayerStat.Money:
                money += modifier;
                break;
            case PlayerStat.Timer:
                gameTimer += modifier;
                break;
            case PlayerStat.Trash:
                trash += modifier;
                if (modifier > 0)
                    for (int i = 0; i < modifier; i++)
                    {
                        CreateTrashBubble();
                    }
                else
                    for (int i = 0; i > modifier; i--)
                    {
                        RemoveTrashBubble();
                    }
                break;
            case PlayerStat.TrashIncrement:
                trashIncrementAmount += modifier;
                break;
            case PlayerStat.TrashIncrementInterval:
                trashIncrementInterval += modifier;
                break;
            case PlayerStat.Illegality:
                illegality += modifier;
                illegalityTimer = 0;
                break;
            default:
                break;
        }
        if (illegality >= 100)
            GameOver("They caught you!", "You should've thought this through more carefully... Welp.");
        if (trash >= 20000)
            GameOver("Too much garbage!", "People have been really good in polluting the planet even more... The planet is unable to bear such amount of trash and it will soon be unable to be inhabited by humans.");
        if (trash <= 0)
            GameOver("All Clean!", "Congratulations! All the trash on this planet has been taken care of, which means we have nothing to fear anymore!");
        UpdateUI();
    }
    public bool TestChangeStats(PlayerStat stat, int modifier)
    {
        switch (stat)
        {
            case PlayerStat.Followers:
                break;
            case PlayerStat.Money:
                if (money + modifier < 0)
                    return false;
                break;
            case PlayerStat.Timer:
                break;
            case PlayerStat.Trash:
                break;
            case PlayerStat.TrashIncrement:
                break;
            case PlayerStat.TrashIncrementInterval:
                break;
            default:
                break;
        }
        return true;
    }
    public IEnumerator StartEventTimer(EventDataScriptable eventData)
    {
        yield return new WaitForSeconds(eventData.time);
        Event tempEvent = Instantiate(eventPrefab, interactiveCanvas.transform);
        if (eventData.mapPosition == Vector2.zero)
            tempEvent.GetComponent<RectTransform>().anchoredPosition = GetPointOnTerrain();
        else
            tempEvent.GetComponent<RectTransform>().anchoredPosition = eventData.mapPosition / 4;
        tempEvent.UpdateEvent(eventData);
        if (eventData.repeatTime > 0)
            StartCoroutine(StartEventTimer(eventData));
    }
    public void UpgradePerk(UpgradeInfo info)
    {
        money -= info.price;
        foreach (var item in info.actions)
        {
            item.Execute();
        }
        switch (info.upgradeType)
        {
            case UpgradeType.Negotiation:
                negotiationLevel++;
                break;
            case UpgradeType.SocialSites:
                socialSitesLevel++;
                break;
            case UpgradeType.Riots:
                riotsLevel++;
                break;
            case UpgradeType.SocialEvents:
                socialEventsLevel++;
                break;
            case UpgradeType.OceanCleansing:
                oceanCleansingLevel++;
                break;
            case UpgradeType.Hacking:
                hackingLevel++;
                break;
            case UpgradeType.Bribe:
                bribeLevel++;
                break;
            case UpgradeType.Blackmail:
                blackmailLevel++;
                break;
            case UpgradeType.Vandalism:
                vandalismLevel++;
                break;
            default:
                break;
        }
        if (oceanCleansingLevel == 5)
            GameOver("Planet Earth is saved!", "You managed to cleanse the oceans, which have been the biggest problem of all! Great job!");
    }
    public void PauseGameToggle(bool value)
    {
        paused = value;
    }
    private void GameOver(string label, string description)
    {
        gameoverScreen.UpdateTexts(label, description);
    }
}
