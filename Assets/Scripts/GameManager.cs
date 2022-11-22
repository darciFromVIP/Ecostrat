using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int money = 500;
    private int followers = 0;
    private float followerIncomeTimer = 0;
    private int illegality = 0;
    [SerializeField] private int trash = 0;
    private float gameTimer = 3600;
    private float bubbleTimer = 0;
    private float trashTimer = 0;
    private float illegalityTimer = 0;
    [SerializeField] private int trashIncrementAmount = 10;
    [SerializeField] private int trashIncrementInterval = 3;
    private List<GameObject> trashBubbles = new();
    private bool paused = false;
    private float oneDayInSec;
    private float dayTimer = 0;
    private int days = 0;
    public int hints = 0;

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
    public FloatingText floatingTextPrefab;

    [Header("UI References")]
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI followerText;
    public TextMeshProUGUI trashText;
    public Slider gameTimerSlider;
    public Slider illegalitySlider;
    public Canvas mapCanvas;
    public Canvas interactiveCanvas;
    public GameOverScreen gameoverScreen;
    public TextMeshProUGUI dayText;
    public RectTransform followersFloatingText;
    public RectTransform moneyFloatingText;
    public RectTransform timeFloatingText;
    public RectTransform illegalityFloatingText;
    public RectTransform trashFloatingText;

    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        Canvas.GetDefaultCanvasMaterial().enableInstancing = true;
        trash = 10000;
        for (int i = 0; i < trash; i++)
        {
            CreateTrashBubble();
        }
        foreach (var item in eventDatabase.events)
        {
            StartCoroutine(StartEventTimer(item));
        }
        StartCoroutine(GameNewsCoroutine());
        StartCoroutine(IllegalityNewsCoroutine());
        oneDayInSec = gameTimer / 365;
        UpdateUI();
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
        if (bubbleTimer >= 5)
        {
            bubbleTimer = 0;
            CreateBubble();
        }
        trashTimer += Time.deltaTime;
        if (trashTimer >= trashIncrementInterval)
        {
            trashTimer = 0;
            ChangeStats(PlayerStat.Trash, trashIncrementAmount);
            for (int i = 0; i < trashIncrementAmount; i++)
            {
                CreateTrashBubble();
            }
            for (int i = 0; i > trashIncrementAmount; i--)
            {
                RemoveTrashBubble();
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
        followerIncomeTimer += Time.deltaTime;
        if (followerIncomeTimer >= 30)
        {
            followerIncomeTimer = 0;
            ChangeStats(PlayerStat.Money, followers);
        }
        dayTimer += Time.deltaTime;
        if (dayTimer >= oneDayInSec)
        {
            dayTimer = 0;
            days++;
            dayText.text = "Day " + days;
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
        trashText.text = trash.ToString();
        illegalitySlider.value = illegality;
    }
    public void AddMoney()
    {
        ChangeStats(PlayerStat.Money, (int)(Random.Range(10, 51) * 3600 / gameTimer));
    }
    public void ChangeStats(PlayerStat stat, int modifier)
    {
        FloatingText text;
        switch (stat)
        {
            case PlayerStat.Followers:
                followers += modifier;
                if (followers < 0)
                    followers = 0;
                text = Instantiate(floatingTextPrefab, followersFloatingText);
                text.UpdateText(modifier.ToString("+#;-#;0"), modifier > 0, true);
                break;
            case PlayerStat.Money:
                money += modifier;
                if (money < 0)
                    money = 0;
                text = Instantiate(floatingTextPrefab, moneyFloatingText);
                text.UpdateText(modifier.ToString("+#;-#;0"), modifier > 0, true);
                break;
            case PlayerStat.Timer:
                gameTimer += modifier;
                text = Instantiate(floatingTextPrefab, timeFloatingText);
                text.UpdateText(modifier.ToString("+#;-#;0") + " seconds", modifier > 0, false);
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
                text = Instantiate(floatingTextPrefab, trashFloatingText);
                text.UpdateText(modifier.ToString("+#;-#;0"), modifier < 0, true);
                break;
            case PlayerStat.TrashIncrement:
                trashIncrementAmount += modifier;
                break;
            case PlayerStat.TrashIncrementInterval:
                trashIncrementInterval += modifier;
                break;
            case PlayerStat.Illegality:
                illegality += modifier;
                if (illegality < 0)
                    illegality = 0;
                illegalityTimer = 0;
                text = Instantiate(floatingTextPrefab, illegalityFloatingText);
                text.UpdateText(modifier.ToString("+#;-#;0"), modifier < 0, true);
                break;
            case PlayerStat.Hint:
                hints += modifier;
                if (hints < 0)
                    hints = 0;
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
        float time = eventData.time;
        while (time > 0)
        {
            if (!paused)
                time -= Time.deltaTime;
            yield return null;
        }
        Event tempEvent = Instantiate(eventPrefab, interactiveCanvas.transform);
        if (eventData.mapPosition == Vector2.zero)
            tempEvent.GetComponent<RectTransform>().anchoredPosition = GetPointOnTerrain();
        else
            tempEvent.GetComponent<RectTransform>().anchoredPosition = eventData.mapPosition / 4;
        tempEvent.UpdateEvent(eventData);
        if (eventData.repeatTime > 0)
            StartCoroutine(StartEventTimer(eventData));
    }
    private IEnumerator GameNewsCoroutine()
    {
        yield return new WaitUntil(() => gameTimer < 2700);
        News.instance.AddMessage("Zost�vaj� ti 3/4 p�vodn�ho �asu, �o sa rovn� presne 273 dn�. Ak m� v pl�ne zachr�ni� Zem, mal by si sa rozh�ba�!");
        yield return new WaitUntil(() => gameTimer < 1800);
        News.instance.AddMessage("Zost�va ti presne polovica p�vodn�ho �asu, �o je presne 182 a p�l d�a. Nechcem mudrova�, ale mysl�m, �e by si naozaj mal za�al robi� zmeny k lep�iemu, lebo inak �el�me z�hube, ak si si to e�te neuvedomil!");
        yield return new WaitUntil(() => gameTimer < 900);
        News.instance.AddMessage("Zost�va ti 1/4 p�vodn�ho �asu, �o je presne 91 dn�, �i�e 3 mesiace. Je mi jedno ako si to chce� vypo��ta�, d�le�it� je len aby si pochopil, �e u� naozaj nem�me �as str�ca� �as!");
        yield return new WaitUntil(() => gameTimer < 600);
        News.instance.AddMessage("Zost�va ti 60 dn�, �i�e 2 mesiace. Z pr�ce si dostal dopis, �e kv�li nepr�jemnej situ�ci� s mno�stvom odpadkov v oce�noch sa va�a pobo�ka zatv�ra. S�ce si aj celkom r�d kv�li v�etkej tej korupci�, ale na druh� stranu prich�dza� o mesa�n� income (- 1 500$ mesa�ne).");
        // Implement money income
        yield return new WaitUntil(() => gameTimer < 300);
        News.instance.AddMessage("Zost�va ti 30 dn�, a. k. a. 1 mesiac, tak sa u� kone�ne rozh�b, inak bude nie len Game Over, ale aj Planeta Zem s �udstvom over!");
        yield return new WaitUntil(() => gameTimer < 180);
        News.instance.AddMessage("Zost�va ti 7 dn� a to znamen� posledn� t��de�! Odpadky zaplavuj� ulice miest. Doomsday klope na dvere!");
        yield return new WaitUntil(() => gameTimer < 60);
        News.instance.AddMessage("Zost�va ti 6 dni! Z mor� a oce�nov sa vyplavuj� tis�cky mrtv�ch r�b zadusen�ch plastami. Rob s t�m pre boha nie�o!");
    }
    private IEnumerator IllegalityNewsCoroutine()
    {
        yield return new WaitUntil(() => illegality >= 20);
        News.instance.AddMessage("Tvoj kamo� Johny bol predvolan� na v�sluch. D�vaj pozor aby si neskon�il tak isto!");
        yield return new WaitUntil(() => illegality >= 40);
        News.instance.AddMessage("Bol si predvolan� na v�sluch! Si ale dobre kryt�, �i�e �iadne ve�k� probl�my ti nehrozia. (m��eme zakomponova� special event 50/50 �e dostane� pokutu or not)");
        yield return new WaitUntil(() => illegality >= 60);
        News.instance.AddMessage("Okolo tvojho bytu sa podozrivo �asto za��na objavova� �ierne BMW a je tam cel� de� aj noc. Skoro to za��na vyzera�, �e n�s zelen� (cops) sleduj�.");
        yield return new WaitUntil(() => illegality >= 80);
        News.instance.AddMessage("Breaking News! The Green Inc. - pomoc plan�te alebo zlo�ineck� organiz�cia?!");
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
            GameOver("Planet Earth is saved!", "You managed to cleanse the oceans, which has been the biggest problem of all! Great job!");
        if (vandalismLevel == 5)
            GameOver("Planet Earth is saved!", "You vandal! :)");
    }
    public void PauseGameToggle(bool value)
    {
        paused = value;
    }
    public bool LegalUltimatePerkUnlocked()
    {
        return negotiationLevel == 5 && socialSitesLevel == 5 && riotsLevel == 5 && socialEventsLevel == 5;
    }
    public bool IllegalUltimatePerkUnlocked()
    {
        return hackingLevel == 5 && bribeLevel == 5 && blackmailLevel == 5;
    }
    private void GameOver(string label, string description)
    {
        gameoverScreen.UpdateTexts(label, description);
    }
}
