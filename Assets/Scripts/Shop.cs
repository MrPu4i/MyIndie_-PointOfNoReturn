using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;



public class Shop : MonoBehaviour //В этом классе делаем всё, что связано с Shop
{
    public List<Implant> AvailableImplants = new List<Implant>(); //доступные импланты
    public bool Lvl2ShopUnlocked = false; //открылись ли эти лвла
    public bool Lvl3ShopUnlocked = false;
    /*public GameObject shopPanel;*/
    public GameObject lvl2ShopPanel;
    public GameObject lvl3ShopPanel;
    public TextMeshProUGUI moneyTextShop;
    public TextMeshProUGUI dayTextShop;
    public GameObject talking_with_master;


    private void Awake() //Каждый раз он создаётся заново
    {
        if (GameManager.Instance.isThereDataInShopData == false)
        {
            Debug.Log("Хотим создать свой Shop");
            InitializeShop();
            Debug.Log(AvailableImplants.Count);
        }
        else
        {
            CopyDataFrom(GameManager.Instance.shopData);
            Debug.Log("Копируем данные");
        }

        UpdateAssortment(); //Обновляем ассортимент при входе
        GameManager.Instance.UpdateUI(); //Уже должны обновить ему
    }
    private void CopyDataFrom(ShopData savedShop)
    {
        // Копируем все нужные поля
        this.AvailableImplants = new List<Implant>(savedShop.availableImplants);
        this.Lvl2ShopUnlocked = savedShop.lvl2ShopUnlocked;
        this.Lvl3ShopUnlocked = savedShop.lvl3ShopUnlocked;
        this.talking_with_master = savedShop.talking_with_master;
    }

    private void InitializeShop()
    {
        AvailableImplants.Add(new Implant("Руки", "arms", 80));
        AvailableImplants.Add(new Implant("Ноги", "legs", 120));
        AvailableImplants.Add(new Implant("Глаза", "eyes", 150));
        AvailableImplants.Add(new Implant("Уши", "ears", 150));
    }

    public void StartShopping()
    {
        talking_with_master.SetActive(false);
    }

    public string UpdateAssortment() //Нужен, когда новые уровни открываются
    {
        // Открываем внутренние органы после 2 имплантов
        if (Player.Instance.implantsCount >= 2 && !Lvl2ShopUnlocked)
        {
            Debug.Log("Открыты внутренние импланты!");

            talking_with_master.SetActive(true); //включаем экран разговора с мастером


            Lvl2ShopUnlocked = true; //открываем 2й левел магазина
            //добавить список новых имплантов
            AvailableImplants.Add(new Implant("Искусственное сердце", "heart", 300));
            AvailableImplants.Add(new Implant("Кибер-лёгкие", "lungs", 250));

            //lvl2 object active ОНО должно автоматически сразу это сделать, я думаю
            lvl2ShopPanel.SetActive(true); //Включаем вторую панель
            return "lvl2Open";
        }

        // Открываем мозг после 5 имплантов
        if (Player.Instance.implantsCount >= 5 && !Lvl3ShopUnlocked)
        {
            talking_with_master.SetActive(true); //включаем экран разговора с мастером

            Lvl3ShopUnlocked = true; //открыли 3й лвл
            AvailableImplants.Add(new Implant("Мозг", "brain", 1000));
            Debug.Log("Открыт Мозг! Осторожно с выбором...");
            //lvl3 object active
            lvl3ShopPanel.SetActive(true);
            return "lvl3Open";
        }
        //ОБНОВЛЯЕМ ПАНЕЛЬ
        return "";
    }

    public void BuyImplant(int index)
    {
        Debug.Log(AvailableImplants);
        //передаёт 0
        Debug.Log("Пытаемся купить что-то");
        if (index < AvailableImplants.Count)
        {
            Debug.Log(AvailableImplants[index].Name);
            string result = Player.Instance.BuyImplant(AvailableImplants[index]); //Мы хотим player передать тот новый implant

            //мы хоть и купили, но надо убрать его из доступных. Достаточно сделать кнопку неактивной

            Debug.Log(result);

            //эту кнопку надо передать и что-то с ней сделать. Недоступной и тёмной
            UpdateShopUI();
        }
        GameManager.Instance.UpdateUI();
    }

    void UpdateShopUI()
    {
        // Здесь обновляем UI магазина
        // Можно создать список кнопок с имплантами
        //здесь появлять панель и обновлять иконки
    }

    public void OnExitButton()
    {
        /*Debug.Log(moneyTextShop);*/
        //Переходим на сцену магазина
        SceneManager.LoadScene("Room"); //По названию сцены
    }
}
