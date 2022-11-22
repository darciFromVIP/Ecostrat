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
        News.instance.AddMessage("Zostávajú ti 3/4 pôvodného èasu, èo sa rovná presne 273 dní. Ak máš v pláne zachráni Zem, mal by si sa rozhýba!");
        yield return new WaitUntil(() => gameTimer < 1800);
        News.instance.AddMessage("Zostáva ti presne polovica pôvodného èasu, èo je presne 182 a pól dòa. Nechcem mudrova, ale myslím, že by si naozaj mal zaèal robi zmeny k lepšiemu, lebo inak èelíme záhube, ak si si to ešte neuvedomil!");
        yield return new WaitUntil(() => gameTimer < 900);
        News.instance.AddMessage("Zostáva ti 1/4 pôvodného èasu, èo je presne 91 dní, èiže 3 mesiace. Je mi jedno ako si to chceš vypoèíta, dôležité je len aby si pochopil, že už naozaj nemáme èas stráca èas!");
        yield return new WaitUntil(() => gameTimer < 600);
        News.instance.AddMessage("Zostáva ti 60 dní, èiže 2 mesiace. Z práce si dostal dopis, že kvôli nepríjemnej situácií s množstvom odpadkov v oceánoch sa vaša poboèka zatvára. Síce si aj celkom rád kvôli všetkej tej korupcií, ale na druhú stranu prichádzaš o mesaèný income (- 1 500$ mesaène).");
        // Implement money income
        yield return new WaitUntil(() => gameTimer < 300);
        News.instance.AddMessage("Zostáva ti 30 dní, a. k. a. 1 mesiac, tak sa už koneène rozhýb, inak bude nie len Game Over, ale aj Planeta Zem s ¾udstvom over!");
        yield return new WaitUntil(() => gameTimer < 180);
        News.instance.AddMessage("Zostáva ti 7 dní a to znamená posledný týždeò! Odpadky zaplavujú ulice miest. Doomsday klope na dvere!");
        yield return new WaitUntil(() => gameTimer < 60);
        News.instance.AddMessage("Zostáva ti 6 dni! Z morí a oceánov sa vyplavujú tisícky mrtvých rýb zadusených plastami. Rob s tým pre boha nieèo!");
    }
    private IEnumerator IllegalityNewsCoroutine()
    {
        yield return new WaitUntil(() => illegality >= 20);
        News.instance.AddMessage("Tvoj kamoš Johny bol predvolaný na výsluch. Dávaj pozor aby si neskonèil tak isto!");
        yield return new WaitUntil(() => illegality >= 40);
        News.instance.AddMessage("Bol si predvolaný na výsluch! Si ale dobre krytý, èiže žiadne ve¾ké problémy ti nehrozia. (môžeme zakomponova special event 50/50 že dostaneš pokutu or not)");
        yield return new WaitUntil(() => illegality >= 60);
        News.instance.AddMessage("Okolo tvojho bytu sa podozrivo èasto zaèína objavova èierne BMW a je tam celý deò aj noc. Skoro to zaèína vyzera, že nás zelení (cops) sledujú.");
        yield return new WaitUntil(() => illegality >= 80);
        News.instance.AddMessage("Breaking News! The Green Inc. - pomoc planéte alebo zloèinecká organizácia?!");
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
