using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Progress;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;
using Unity.VisualScripting;
using System.Collections;


public class Shop : MonoBehaviour //В этом классе делаем всё, что связано с Shop
{


    [HideInInspector] public List<Implant> AvailableImplants = new List<Implant>(); //доступные импланты
    public bool Lvl2ShopUnlocked = false; //открылись ли эти лвла
    public bool Lvl3ShopUnlocked = false;

    public GameObject shopPanel; //для выключения во время разговора с мастером
    public GameObject playerPanel; //для выключения во время разговора с мастером
    public GameObject lvl2ShopPanel;
    public GameObject lvl3ShopPanel;
    public TextMeshProUGUI moneyTextShop;
    public TextMeshProUGUI dayTextShop;
    public GameObject talking_with_master;
    public GameObject meds_yes;
    public GameObject meds_no;
    public ImplantsRendere implantsRenderer;
    

    public GameObject[] implantsInShop; //Каждый раз заходя я должна с этим списком свериться
    public GameObject[] lvls;
    public ShopData sd;



    private void Awake() //Каждый раз он создаётся заново
    {
        Debug.Log(GameManager.Instance.isThereDataInShopData);
        if (GameManager.Instance.isThereDataInShopData == false)
        {
            //Debug.Log("Хотим создать свой Shop");
            InitializeShop();
            //Debug.Log(AvailableImplants.Count);
            MasterDialogStart("Первый раз");

            GameManager.Instance.isThereDataInShopData = true;
        }
        else
        {
            CopyDataFrom(GameManager.Instance.shopData);
            //Debug.Log("Копируем данные");
        }

        //Debug.Log(meds_yes.activeInHierarchy);
        meds_yes.SetActive(false);
        //Debug.Log(meds_yes.activeInHierarchy);
        meds_no.SetActive(false);


        UpdateShopUI();
        UpdateAssortment(); //Обновляем ассортимент при входе
        GameManager.Instance.UpdateUI(); //Уже должны обновить ему
    }
    private void CopyDataFrom(ShopData savedShop)
    {
        Debug.Log("Происходит копирование В Shop!!");
        // Копируем все нужные поля
        Debug.Log(savedShop.availableImplants);
        this.AvailableImplants = new List<Implant>(savedShop.availableImplants);
        sd = new ShopData();
        sd.LoadData();
        Lvl2ShopUnlocked = sd.lvl2ShopUnlocked;
        Lvl2ShopUnlocked = sd.lvl2ShopUnlocked;
    }

    private void CopyDataTo(ShopData savedShop)
    {
        Debug.Log("Происходит копирование ИЗ Shop!!");
        savedShop.availableImplants = new List<Implant>(this.AvailableImplants);
        sd.lvl2ShopUnlocked = Lvl2ShopUnlocked;
        sd.lvl3ShopUnlocked = Lvl3ShopUnlocked;
        sd.SaveData();
    }

    private void InitializeShop() //Сразу весь магазин инициализируем
    {
        AvailableImplants.Add(new Implant("Техно Руки", "arms", 25, "Sprites/T-ruki", new Vector2(0,0)));
        AvailableImplants.Add(new Implant("Техно Ноги", "legs", 15, "Sprites/T-nogi", new Vector2(0, 0)));
        AvailableImplants.Add(new Implant("Техно Глаза", "eyes", 30, "Sprites/T-glaza", new Vector2(0, 0)));
        AvailableImplants.Add(new Implant("Техно Уши", "ears", 10, "Sprites/T-ushi", new Vector2(0, 0)));
        AvailableImplants.Add(new Implant("Искусственное сердце", "heart", 110, "Sprites/hand", new Vector2(356, 539)));
        AvailableImplants.Add(new Implant("Кибер-лёгкие", "lungs", 80, "Sprites/hand", new Vector2(290, 506)));
        AvailableImplants.Add(new Implant("Техно-Печень", "liver", 60, "Sprites/hand", new Vector2(356, 510)));
        AvailableImplants.Add(new Implant("Техно-Почки", "kidneys", 45, "Sprites/hand", new Vector2(320, 506)));
        AvailableImplants.Add(new Implant("Техно Мозг", "brain", 1000, "Sprites/hand", new Vector2(1, 3)));

        /*AvailableImplants.Add(new Implant("Техно Руки", "arms", 25, "Sprites/T-ruki", new Vector2(310, 598)));
        AvailableImplants.Add(new Implant("Техно Ноги", "legs", 15, "Sprites/T-nogi", new Vector2(314, 198)));
        AvailableImplants.Add(new Implant("Техно Глаза", "eyes", 30, "Sprites/T-glaza", new Vector2(313, 775)));
        AvailableImplants.Add(new Implant("Техно Уши", "ears", 10, "Sprites/T-ushi", new Vector2(313, 758)));
        AvailableImplants.Add(new Implant("Искусственное сердце", "heart", 110, "Sprites/hand", new Vector2(356, 539)));
        AvailableImplants.Add(new Implant("Кибер-лёгкие", "lungs", 80, "Sprites/hand", new Vector2(290, 506)));
        AvailableImplants.Add(new Implant("Техно-Печень", "liver", 60, "Sprites/hand", new Vector2(356, 510)));
        AvailableImplants.Add(new Implant("Техно-Почки", "kidneys", 45, "Sprites/hand", new Vector2(320, 506)));
        AvailableImplants.Add(new Implant("Техно Мозг", "brain", 1000, "Sprites/hand", new Vector2(1, 3)));*/
    }

    public void BringToFront(int layerIndex) //Какую мы поставим
    {
        lvls[layerIndex].transform.SetAsLastSibling();
    }

    public void MasterDialogEnd()
    {
        talking_with_master.SetActive(false);
        playerPanel.SetActive(true);
        shopPanel.SetActive(true);
    }
    public void MasterDialogStart(string what) //Смотреть какой это именно случай
    {
        Debug.Log(what);
        talking_with_master.SetActive(true); //включаем экран разговора с мастером
        playerPanel.SetActive(false);
        shopPanel.SetActive(false);

        if (what == "Открыт lvl2")
        {
            string[] texts = {
            "Эй, ты так много имплантов поставил! Наверное программа этой корпорации хорошо работает..",
            "Вот завезли мне ещё экземпляров, попробуй их, думаю за это хорошо будут платить",
            "Дайвайте взглянем"
            };

            string[] speakers = {
            "Мастер",
            "Мастер",
            "Владик"
            };

            DialogueManager.Instance.StartDialogueFromCode(texts, speakers, true); //Начинаем этот диалог
        }
        if (GameManager.Instance.isThisFirstTime == true)
        {
            //Только в первый раз такой разговор
            string[] texts = {
            "Есть кто?",

            "....*&!^#%&$^ ух...",
            "Да да? Я здесь",

            "Здравствуйте! Я хотел бы записаться на тестирование имплантов",

            "Э.. да да, можно, давайте",

            "А у вас точно всё безопасно?",

            ".мээ ну как ещё же, да.. Проходите",
            "э но-   только по одному импланту в день.. Больше Я не хочу делать кхмм"
                };

            string[] speakers = {
            "Владик",
            "Мастер",
            "Мастер",
            "Владик",
            "Мастер",
            "Владик",
            "Мастер",
            "Мастер"
                };

            DialogueManager.Instance.StartDialogueFromCode(texts, speakers, true); //Начинаем этот диалог
            GameManager.Instance.isThisFirstTime = false;
        }
        else if (what == "Открыт lvl3")
        {
            string[] texts = {
            "Постой, слушай...",
            "В последнее время ты и вправду устанавливаешь много имплантов",
            "Так что только для тебя от корпорации пришёл 'особый' товар",
            "Для меня? Странно.. Это же не опасно?",
            "  н-нет...   я  же",
            "Мастер на все руки",
            "...",
            "Ладно"
            };

            string[] speakers = {
            "Мастер",
            "Мастер",
            "Мастер",
            "Владик",
            "Мастер",
            "Мастер",
            "Владик",
            "Владик"
            };

            DialogueManager.Instance.StartDialogueFromCode(texts, speakers, true); //Начинаем этот диалог
        }
        else if (what == "Таблетка")
        {
            //Debug.Log(GameManager.Instance.isThisFirstTimeMakingImplants);
            if (GameManager.Instance.isThisFirstTimeMakingImplants == true)
            {
                //Только первый раз
                string[] texts1 = {
                "Операция прошла успешно, прими таблетку от побочек"
                };

                DialogueManager.Instance.StartDialogueFromCodeSolo(texts1, "Мастер");

                //Появляются кнопки да и нет
                meds_yes.SetActive(true);
                meds_no.SetActive(true);
            }
        }
        //Скрипт как он говорит - выбор таблетки - опять диалог, выбор между двумя
        //Мы либо с мастером говорим, либо какое-то комментарии в комнате своей,но немного такого будет. В самом начале и потом на таблетку коммент 1 раз
        //Может быть когда он рассказывает про новую вкладку
        //Может быть после операции (предлагает таблетку). Включаем кнопки Принять и не принимать. Кнопку далее убираем. После нажатия - ещё диалог
    }

    public void ButtonYes()
    {
        SceneManager.LoadScene("Room"); //Просто тепаемся домой. Ну переход с затемнением
    }

    public void ButtonNo()
    {
        SceneManager.LoadScene("Room");
    }
    public void UpdateAssortment() //Нужен, когда новые уровни открываются
    {
        // Открываем внутренние органы после 2 имплантов
        if (Player.Instance.implantsCount >= 2 && !Lvl2ShopUnlocked)
        {
            Debug.Log("Открыты внутренние импланты!");
            MasterDialogStart("Открыт lvl2");

            Lvl2ShopUnlocked = true; //открываем 2й левел магазина
            //добавить список новых имплантов

            lvl2ShopPanel.SetActive(true); //Включаем вторую панель
        }

        // Открываем мозг после 5 имплантов
        if (Player.Instance.implantsCount >= 4 && !Lvl3ShopUnlocked) 
        {
            Debug.Log("Открыт Мозг! Осторожно с выбором...");
            MasterDialogStart("Открыт lvl3");

            Lvl3ShopUnlocked = true; //открыли 3й лвл

            //lvl3 object active
            lvl2ShopPanel.SetActive(true);
            lvl3ShopPanel.SetActive(true);
        }
    }

    public void BuyImplant(int index, GameObject item)
    {
        if (GameManager.Instance.didWeByeSmth == true) //Если мы уже что-то купили, ничего не можем делать
        {
            return;
        }
        if (index < AvailableImplants.Count)
        {
            string result = Player.Instance.BuyImplant(AvailableImplants[index]); //Мы хотим player передать тот новый implant

            if (result != "Недостаточно денег.")
            {
                AvailableImplants[index].IsSold = true; //МЫ ПРОДАЛИ ЭТОТ ИМПЛАНТ'
                implantsRenderer.RefreshImplantDisplay();
                UpdateShopUI();
            }
            Debug.Log(result);

            //эту кнопку надо передать и что-то с ней сделать. Недоступной и тёмной

        }
        GameManager.Instance.UpdateUI();
    }

    void InactiveImplant(GameObject item)
    {
        //мы хоть и купили, но надо убрать его из доступных. Достаточно сделать кнопку неактивной
        TextMeshProUGUI[] itemTexts = GetComponentsInChildren<TextMeshProUGUI>();
        //Надо затемнять, а не делать прозрачным
        foreach (TextMeshProUGUI text in itemTexts)
        {
            Color currentColor = text.color;
            currentColor.a = 0.5f;
            text.color = currentColor;
        }

        Button but = item.GetComponentInChildren<Button>();
        but.enabled = false;
        UnityEngine.UI.Image buttonImage = but.GetComponent<UnityEngine.UI.Image>();
        if (buttonImage != null)
        {
            buttonImage.color = Color.gray;
        }

        Color curColor = buttonImage.color;
        curColor.a = 0.5f;
        buttonImage.color = curColor;
    }

    void UpdateShopUI()
    {
        foreach (var implant in AvailableImplants) //По всем доступным в магазине
        {
            //Debug.Log($"{implant.Name},{implant.IsSold}");
            if (implant.IsSold == true)//если по проданному прошлись - применяем стили к нему
            {
                GameObject item = implantsInShop[AvailableImplants.IndexOf(implant)]; //ЭТО у нас ShopImplant в котором есть и индекс и сам объект. ЭТО ОБЪЕКТ
                InactiveImplant(item); //И эту ячейку мы затемняем
            }
        }
    }

    public void OnExitButton()
    {
        /*if (GameManager.Instance.didWeByeSmth == true) //значит мы что-то купили, когда выходим
        {
            MasterDialogStart("Таблетка");
        }*/
        /*else
        {*/
            CopyDataTo(GameManager.Instance.shopData);
            /*MasterDialogEnd();*/
            SceneManager.LoadScene("Room"); //По названию сцены
        /*}*/
    }
}
