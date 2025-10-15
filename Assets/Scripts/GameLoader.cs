using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameLoader : MonoBehaviour
{
    public GameObject gameManagerPrefab;
    public GameObject playerPrefab;

    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI dayText;
    public GameObject gameOverPanel;

    public Button sleep;
    public Button work;
    public Button shop;

    void Awake()
    {
        //Debug.Log("Я GameLoader и я создаюсь!");
        // Если Player не существует - создаем
        if (Player.Instance == null)
        {
            Instantiate(playerPrefab);
            //Debug.Log("Инициализируем ПЛЕЕРА");
        }

        // Если GameManager не существует - создаем
        if (GameManager.Instance == null)
        {
            //Debug.Log("Инициализируем ГЕЙММЕНЕДЖЕР");
            GameManager gm = Instantiate(gameManagerPrefab).GetComponent<GameManager>();
        }
        if (SceneManager.GetActiveScene().name == "Room")
        { 
            sleep.onClick.AddListener(GameManager.Instance.OnSleepButton);
            work.onClick.AddListener(GameManager.Instance.OnWorkButton);
            shop.onClick.AddListener(GameManager.Instance.OnShopButton);
        }
    }
}
