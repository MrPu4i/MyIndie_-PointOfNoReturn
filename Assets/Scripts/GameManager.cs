using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

[System.Serializable]
public class ShopData
{
    public List<Implant> availableImplants;
    public bool lvl2ShopUnlocked;
    public bool lvl3ShopUnlocked;

    public void SaveData()
    {
        PlayerPrefs.SetInt("ShopLvl2", lvl2ShopUnlocked ? 1 : 0);
        PlayerPrefs.SetInt("ShopLvl3", lvl3ShopUnlocked ? 1 : 0);
        PlayerPrefs.Save();
    }

    // Метод для загрузки данных
    public void LoadData()
    {
        lvl2ShopUnlocked = PlayerPrefs.GetInt("ShopLvl2", 0) == 1;
        lvl3ShopUnlocked = PlayerPrefs.GetInt("ShopLvl3", 0) == 1;
    }
}

    public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int Day = 1; //Начинаем с первого дня
    public int maxDays;
    public bool GameOver = false;
    public ShopData shopData;
    public bool isThereDataInShopData = false;
    public bool isThisFirstShopPanel = true;


    //Исторические
    public bool isThisFirstTimeMakingImplants = false;
    public bool isThisFirstTime = true;
    [HideInInspector] public bool didWeByeSmth = false;
    public bool eyeIsSold; //Купили глаз

    // UI элементы
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI dayText;
    /*public GameObject gameOverPanel;*/
    /*public TextMeshProUGUI gameOverText;*/


    public GameObject sp;


    void Awake()
    {
        //Debug.Log($"GameManager Awake! ID: {GetInstanceID()}");

        if (Instance != null && Instance != this)
        {
            //Debug.Log($"УНИЧТОЖАЮ ДУБЛИКАТ! Старый ID: , Новый ID: {GetInstanceID()}");
            Destroy(gameObject);
            return;
        }

        maxDays = 20;
        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Canvas canvas = FindFirstObjectByType<Canvas>();
        //ПРи каждом переходе сцены!!
        if (scene.name == "Room") //В первый раз
        {
            didWeByeSmth = false;

            if (isThisFirstTime == true)
            {
                string[] texts = {
            "Тестирование имплантов.. Звучит рисково, но я как раз хотел накопить деньги на отпуск",
            "Как раз здесь недалеко есть их отделение, нужно туда сходить."
            };

                /*Debug.Log($"StartDialogueSolo: {texts}");*/
                DialogueManager.Instance.StartDialogueFromCodeSolo(texts, "Владик");
            }
            //После таблеток

        }
        if (scene.name == "Shop")
        {
            didWeByeSmth = false; //Сбрасываем покупку
        }
        Debug.Log($"Загружена сцена: {scene.name}");
        
        moneyText = canvas.transform.Find("Text_money").GetComponent<TextMeshProUGUI>();
        dayText = canvas.transform.Find("Text_day").GetComponent<TextMeshProUGUI>();
        UpdateUI();
    }

    void Start()
    {
        /*Canvas canvas = FindFirstObjectByType<Canvas>();
        moneyText = canvas.transform.Find("Text_money").GetComponent<TextMeshProUGUI>();
        dayText = canvas.transform.Find("Text_day").GetComponent<TextMeshProUGUI>();*/
        //gameOverPanel = canvas.transform.Find("GameOver").GetComponent<GameObject>();
        //Debug.Log($"{moneyText}, {dayText}, {gameOverPanel}");
        
    }

    void OnEnable()
    {
        //Debug.Log("Я геймменеджер и я OnEnable");
    }

    public void OnWorkButton()
    {
        string result = Player.Instance.Work();
        //Debug.Log(result);
        UpdateUI();
    }

    public void OnSleepButton()
    {
        string result = Player.Instance.Sleep();
        //Debug.Log(result);
        StartNewDay(); //добавляем 1 день
    }

    public void StartNewDay()
    {
        if (Day >= maxDays)
            EndGame(); //Дни истекли
        if (GameOver) return;

        Day++;
        UpdateUI();
    }

    public void OnShopButton()
    {
        SceneManager.LoadScene("Shop"); //По названию сцены
    }

    public void EndGame()
    {
        if (Day >= maxDays)
        {
            if (Player.Instance.Money == Player.Instance.goal) //Набили нужную сумму
            {
                //Happy Ending на море
            }
        }
            GameOver = true;
        //gameOverPanel.SetActive(true);
    }

    public void UpdateUI()
    {
        moneyText.text = $"Деньги: {Player.Instance.Money}";
        dayText.text = $"День: {Day}";
    }

}
