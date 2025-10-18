using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    //Технические
    public ShopData shopData;
    public bool isThereDataInShopData = false;
    public bool isThisFirstShopPanel = true;

    public int lvlSelected = -1;
    public int dayCountForEye = 0;


    //Исторические
    public bool isThisFirstTimeMakingImplants = false;
    public bool isThisFirstTime = true;
    [HideInInspector] public bool didWeByeSmth = false;
    public bool eyeIsSold; //Купили глаз
    public bool GameOver = false;


    // UI элементы
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI dayText;
    public TextMeshProUGUI freedomText;

    public GameObject texts;


    public GameObject gameOverPanel;
    public GameObject gameOverButton;



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

        maxDays = 12;
        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    public void FindTextElementsWithoutActivation()
    {
        // Метод, который не меняет активность объектов
        moneyText = FindComponentInChildren<TextMeshProUGUI>(texts.transform, "Text_money");
        dayText = FindComponentInChildren<TextMeshProUGUI>(texts.transform, "Text_day");
        freedomText = FindComponentInChildren<TextMeshProUGUI>(texts.transform, "Text_freedom");
    }

    private T FindComponentInChildren<T>(Transform parent, string childName) where T : Component
    {
        // Ручной обход детей без использования GetComponentsInChildren
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);

            if (child.name == childName)
            {
                T component = child.GetComponent<T>();
                if (component != null) return component;
            }

            // Рекурсивно проверяем детей
            T foundInChildren = FindComponentInChildren<T>(child, childName);
            if (foundInChildren != null) return foundInChildren;
        }

        return null;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        gameOverPanel = GameObject.Find("GameOver");
        Canvas[] allCanvases = Resources.FindObjectsOfTypeAll<Canvas>();
        //ПРи каждом переходе сцены!!

        Debug.Log(allCanvases);
        Canvas canvas = GameObject.FindFirstObjectByType<Canvas>();
        Debug.Log(canvas);
        if (canvas != null)
        {
            GameObject textss = null;
            GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

            foreach (GameObject obj in allObjects)
            {
                if (obj.name == "Texts" && obj.scene.isLoaded)
                {
                    textss = obj;
                    break;
                }
            }
            Debug.Log(textss);
            texts = textss;
            GameObject textst = GameObject.Find("Texts");
            Debug.Log(textst); //null
            FindTextElementsWithoutActivation();
            /*texts = GameObject.Find("Texts");*/
            TextMeshProUGUI[] allTexts = texts.GetComponentsInChildren<TextMeshProUGUI>(true);

            foreach (TextMeshProUGUI text in allTexts)
            {
                switch (text.name)
                {
                    case "Text_money":
                        moneyText = text;
                        break;
                    case "Text_day":
                        dayText = text;
                        break;
                    case "Text_freedom":
                        freedomText = text;
                        break;
                }
            }
        }

        if (scene.name == "Room") //В первый раз
        {
            if (isThisFirstTime == true)
            {
                string[] texts = {
            "Тестирование имплантов.. Звучит рисково, но я как раз хотел накопить деньги на отпуск",
            "Здесь недалеко есть их отделение, нужно туда сходить",
            "Ещё Санёк написал... Что-то там про безопасность хмм"
            };

                /*Debug.Log($"StartDialogueSolo: {texts}");*/
                DialogueManager.Instance.StartDialogueFromCodeSolo(texts, "Владик");
            }
            //После таблеток

        }
        if (scene.name == "Shop")
        {
        }
        Debug.Log($"Загружена сцена: {scene.name}");


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
        if (Player.Instance.HasWorkedToday == true) //Уже работали
        {
            string[] texts = {
            "Я уже поработал сегодня"
            };

            DialogueManager.Instance.StartDialogueFromCodeSolo(texts, "Владик");
            return;
        }
        string result = Player.Instance.Work();
        //Debug.Log(result);
        UpdateUI();
    }

    public void OnSleepButton()
    {
        if (Player.Instance.HasWorkedToday == false)
        {
            string[] texts = {
            "Я не могу лечь спать, не поработав"
            };

            DialogueManager.Instance.StartDialogueFromCodeSolo(texts, "Владик");
            return;
        }
        string result = Player.Instance.Sleep();
        //Debug.Log(result);
        StartNewDay(); //добавляем 1 день
    }

    public void StartNewDay()
    {
        if (Day >= maxDays)
            EndGame("Дни истекли"); //Дни истекли
        if (GameOver) return;

        Day++;
        if (eyeIsSold)
        {
            dayCountForEye++;
        }
        UpdateUI();
    }

    public void OnShopButton()
    {
        SceneManager.LoadScene("Shop"); //По названию сцены
    }

    public void EndGame(string situation)
    {
        gameOverPanel = GameObject.Find("GameOver");
        gameOverButton.SetActive(true);//Включаем кнопку рестарта

        Debug.Log(gameOverPanel.transform.Find("GameLoosSad").gameObject);
        //Затемнение
        if (situation == "Мозг") //Нас тепают сюда в момент покупки мозга
        {
            //Самая плохая концовка
            gameOverPanel.transform.Find("GameLoosSad").gameObject.SetActive(true);
            //Звук
        }
        if (eyeIsSold)
        {
            if (situation == "Дни истекли") //Дни истекли и мы накопили нужную сумму
            {
                if (Player.Instance.Money >= Player.Instance.goal) //Набили нужную сумму
                {
                    //Happy Ending на море
                    gameOverPanel.transform.Find("GameLoosHappy").gameObject.SetActive(true); //С экраном типо всё хорошо
                    //Звук

                }
                else
                {
                    gameOverPanel.transform.Find("GameLoos").gameObject.SetActive(true);
                    //Звук
                }
            }
        }
        else 
        {
            if (situation == "Дни истекли") //Дни истекли и мы накопили нужную сумму
            {
                if (Player.Instance.Money >= Player.Instance.goal) //Набили нужную сумму
                {
                    //Happy Ending на море
                    gameOverPanel.transform.Find("GameWin").gameObject.SetActive(true);
                    //Звук

                }
                else
                {
                    gameOverPanel.transform.Find("GameLoosNoMoney").gameObject.SetActive(true);
                    //Звук
                }
            }
        }
        gameOverPanel.SetActive(true);
        //Включаем скрипт Гейм овера
    }

    public void UpdateUI()
    {
        Debug.Log("Пытаемся отрисовать данные");
        dayText.text = $"{Day} / {maxDays}";
        moneyText.text = $"{Player.Instance.earnings} $"; //+ с имплантов
        freedomText.text = $"{Player.Instance.Money} / {Player.Instance.goal} $"; // 1504 / 3200 (цель)

    }

}
