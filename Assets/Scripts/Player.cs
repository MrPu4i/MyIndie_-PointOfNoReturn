using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public static Player Instance;

    public int Money = 600;
    public int implantsCount = 0;
    public Dictionary<string, Implant> Implants;
    public bool HasWorkedToday = false;
    public int earnings = 0;
    public int goal;

    void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            //Debug.Log("Уничтожаем дубликат Player!");
            Destroy(gameObject);
            return;
        }

        goal = 3000;
        Implants = new Dictionary<string, Implant>()
        {
        {"arms", new Implant("Руки", "arms", 25, "Sprites/Ruki", new Vector2 (0,0))},
        {"legs", new Implant("Ноги", "legs", 15, "Sprites/Nogi", new Vector2 (0,0))},
        {"eyes", new Implant("Глаза", "eyes", 30, "Sprites/Glaza", new Vector2 (0,0))},
        {"ears", new Implant("Уши", "ears", 10, "Sprites/Ushi", new Vector2 (0,0))},
        {"heart", new Implant("Сердце", "heart", 110, "Sprites/normal_hand", new Vector2 (356,539))},
        {"lungs", new Implant("Лёгкие", "lungs", 80, "Sprites/normal_hand", new Vector2 (290,506))},
        {"liver", new Implant("Печень", "liver", 60, "Sprites/normal_hand", new Vector2 (356,510))},
        {"kidneys", new Implant("Почки", "kidneys", 45, "Sprites/normal_hand", new Vector2 (320,506))}
        };
        Instance = this;
        DontDestroyOnLoad(gameObject);
        //Debug.Log("Player создан и сохранен!");
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
    public string Work()
    {
        if (!HasWorkedToday)
        {
            Money += 50; //Просто приносит деньги
            //Чем больше имплантов, тем больше зарабатываете
            HasWorkedToday = true;
            return "Вы поработали и заработали 50 монет.";
        }
        return "Вы уже работали сегодня.";
    }

    public string Sleep()
    {
        HasWorkedToday = false; //обновилли силы
        //Начислили проценты
        Money += earnings;
        return "Вы выспались и полны сил.";
    }

    public string BuyImplant(Implant implant)
    {
        if (Money >= implant.Price)
        {
            implantsCount++;
            Money += implant.Price;
            earnings += implant.Price; //Увеличивается наш процент каждадневный

            //Debug.Log($"Что-то купили {GameManager.Instance.didWeByeSmth}");
            GameManager.Instance.didWeByeSmth = true;

            if (GameManager.Instance.isThisFirstTimeMakingImplants == false)
            {
                //1 Раз только должен зайти
                GameManager.Instance.isThisFirstTimeMakingImplants = true; //И сразу фолз. Чтобы в шопе мы могли диалог другой в первый раз поставить
            }
            Implants[implant.Slot] = implant; //заменяем имплант

            //в этот момент обновить Player
            /*UpdateBodyVisual(implant.Slot);*/

            // Если купили мозг - игра окончена
            if (implant.Slot == "brain")
            {
                GameManager.Instance.EndGame();
                return "Вы поменяли мозг... Кто вы теперь?";
            }

            return $"Вы купили и установили имплант {implant.Name}";
        }
        return "Недостаточно денег.";
    }
}

