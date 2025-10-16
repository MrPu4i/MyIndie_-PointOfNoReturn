using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Progress;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;
using Unity.VisualScripting;


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
    public ImplantsRendere implantsRenderer;
    [HideInInspector] public bool didWeByeSmth = false;

    public GameObject[] implantsInShop; //Каждый раз заходя я должна с этим списком свериться
    public GameObject[] lvls;



    private void Awake() //Каждый раз он создаётся заново
    {
        Debug.Log(GameManager.Instance.isThereDataInShopData);
        if (GameManager.Instance.isThereDataInShopData == false)
        {
            Debug.Log("Хотим создать свой Shop");
            InitializeShop();
            Debug.Log(AvailableImplants.Count);

            GameManager.Instance.isThereDataInShopData = true;
        }
        else
        {
            CopyDataFrom(GameManager.Instance.shopData);
            Debug.Log("Копируем данные");
        }
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
        this.Lvl2ShopUnlocked = savedShop.lvl2ShopUnlocked;
        this.Lvl3ShopUnlocked = savedShop.lvl3ShopUnlocked;
    }

    private void CopyDataTo(ShopData savedShop)
    {
        Debug.Log("Происходит копирование ИЗ Shop!!");
        savedShop.availableImplants = new List<Implant>(this.AvailableImplants);
        bool lvl2Un = this.Lvl2ShopUnlocked;
        savedShop.lvl2ShopUnlocked = lvl2Un;
        bool lvl3Un = this.Lvl3ShopUnlocked;
        savedShop.lvl3ShopUnlocked = lvl3Un;
    }

    private void InitializeShop() //Сразу весь магазин инициализируем
    {
        AvailableImplants.Add(new Implant("Техно Руки", "arms", 80, "Sprites/hand", new Vector2 (310, 598)));
        AvailableImplants.Add(new Implant("Техно Ноги", "legs", 120, "Sprites/hand", new Vector2(314, 198)));
        AvailableImplants.Add(new Implant("Техно Глаза", "eyes", 150, "Sprites/hand", new Vector2(313, 775)));
        AvailableImplants.Add(new Implant("Техно Уши", "ears", 150, "Sprites/hand", new Vector2(313, 758)));
        AvailableImplants.Add(new Implant("Искусственное сердце", "heart", 300, "Sprites/hand", new Vector2(356, 539)));
        AvailableImplants.Add(new Implant("Кибер-лёгкие", "lungs", 250, "Sprites/hand", new Vector2(290, 506)));
        AvailableImplants.Add(new Implant("Техно-Печень", "liver", 80, "Sprites/hand", new Vector2(356, 510)));
        AvailableImplants.Add(new Implant("Техно-Почки", "kidneys", 80, "Sprites/hand", new Vector2(320, 506)));
        AvailableImplants.Add(new Implant("Техно Мозг", "brain", 1000, "Sprites/hand", new Vector2(1, 3)));
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
    public void MasterDialogStart()
    {
        talking_with_master.SetActive(true); //включаем экран разговора с мастером
        playerPanel.SetActive(false);
        shopPanel.SetActive(false);
    }

    public void UpdateAssortment() //Нужен, когда новые уровни открываются
    {
        // Открываем внутренние органы после 2 имплантов
        if (Player.Instance.implantsCount >= 2 && !Lvl2ShopUnlocked)
        {
            Debug.Log("Открыты внутренние импланты!");

            MasterDialogStart();

            Lvl2ShopUnlocked = true; //открываем 2й левел магазина
            //добавить список новых имплантов

            lvl2ShopPanel.SetActive(true); //Включаем вторую панель
        }

        // Открываем мозг после 5 имплантов
        if (Player.Instance.implantsCount >= 5 && !Lvl3ShopUnlocked) 
        {
            Debug.Log("Открыт Мозг! Осторожно с выбором...");
            MasterDialogStart();

            Lvl3ShopUnlocked = true; //открыли 3й лвл

            //lvl3 object active
            lvl3ShopPanel.SetActive(true);
        }
    }

    public void BuyImplant(int index, GameObject item)
    {
        if (index < AvailableImplants.Count)
        {
            didWeByeSmth = true; //Мы что-то купили, может несколько раз подряд
            //Debug.Log(AvailableImplants[index].Name);
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
        /*Debug.Log(moneyTextShop);*/
        //Переходим на сцену магазина
        if (didWeByeSmth == true) //значит мы что-то купили, когда выходим
        {
            //запускается диалог с мастером
            MasterDialogStart();

            //Выбор кнопки
            //Когда он заканчивается
            didWeByeSmth = false;
        }
        else
        {
            CopyDataTo(GameManager.Instance.shopData);
            MasterDialogEnd();
            SceneManager.LoadScene("Room"); //По названию сцены
        }
    }
}
