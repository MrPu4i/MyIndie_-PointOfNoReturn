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
    public GameObject talking_with_master;
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int Day = 1; //Начинаем с первого дня
    public bool GameOver = false;
    public ShopData shopData;
    public bool isThereDataInShopData = false;

    // UI элементы
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI dayText;
    /*public GameObject gameOverPanel;*/
    /*public TextMeshProUGUI gameOverText;*/


    void Awake()
    {
        //Debug.Log($"GameManager Awake! ID: {GetInstanceID()}");

        if (Instance != null && Instance != this)
        {
            //Debug.Log($"УНИЧТОЖАЮ ДУБЛИКАТ! Старый ID: , Новый ID: {GetInstanceID()}");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Загружена сцена: {scene.name}");
        Canvas canvas = FindFirstObjectByType<Canvas>();
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
        Debug.Log(result);
        UpdateUI();
    }

    public void OnSleepButton()
    {
        string result = Player.Instance.Sleep();
        Debug.Log(result);
        StartNewDay(); //добавляем 1 день
    }

    public void StartNewDay()
    {
        if (GameOver) return;

        Day++;
        UpdateUI();
    }

    public void OnShopButton()
    {
        SceneManager.LoadScene("Shop"); //По названию сцены
        UpdateUI();
    }

    public void OnRoomEnter()
    {
    }

    public void EndGame()
    {
        GameOver = true;
        //gameOverPanel.SetActive(true);
    }

    public void UpdateUI()
    {
        //Debug.Log(moneyText);
        moneyText.text = $"Деньги: {Player.Instance.Money}";
        dayText.text = $"День: {Day}";
    }

}
