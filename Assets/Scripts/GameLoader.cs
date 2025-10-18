using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameLoader : MonoBehaviour
{
    public GameObject gameManagerPrefab;
    public GameObject playerPrefab;

    public GameObject adds;
    public GameObject gameOverButtonL;
    GameManager gm;

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
            gm = Instantiate(gameManagerPrefab).GetComponent<GameManager>();
        }
        if (SceneManager.GetActiveScene().name == "Room")
        { 
            sleep.onClick.AddListener(GameManager.Instance.OnSleepButton);
            work.onClick.AddListener(GameManager.Instance.OnWorkButton);
            shop.onClick.AddListener(GameManager.Instance.OnShopButton);
        }
        GameManager.Instance.gameOverButton = gameOverButtonL;
        Debug.Log(GameManager.Instance.gameOverButton);
    }

    public void RestartGame()
    {//Начать всё сначала
        Debug.Log("click");
        GameManager.Instance.shopData.availableImplants = null;
        GameManager.Instance.shopData.lvl2ShopUnlocked = false;
        GameManager.Instance.shopData.lvl3ShopUnlocked = false;
        if (GameManager.Instance.gameObject != null)
        {
            Destroy(GameManager.Instance.gameObject);
            Destroy(Player.Instance.gameObject);
            Destroy(DialogueManager.Instance.gameObject);
        }
        GameManager.Instance = null;
        Player.Instance = null;
        DialogueManager.Instance = null;

        Resources.UnloadUnusedAssets();
        SceneManager.LoadScene("Menu");
    }

    public void AddsOn()
    {
        adds.SetActive(true);
    }

    public void AddsOff()
    {
        adds.SetActive(false);
    }
}
