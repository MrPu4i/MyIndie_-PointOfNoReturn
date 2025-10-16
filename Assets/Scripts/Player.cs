using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public static Player Instance;

    public int Money = 600;
    public int implantsCount = 0;
    public Dictionary<string, Implant> Implants;
    public bool HasWorkedToday = false;

    void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Debug.Log("Уничтожаем дубликат Player!");
            Destroy(gameObject);
            return;
        }

        Implants = new Dictionary<string, Implant>()
        {
        {"arms", new Implant("Руки", "arms", 80, "Sprites/normal_hand", new Vector2 (310,598))},
        {"legs", new Implant("Ноги", "legs", 80, "Sprites/normal_hand", new Vector2 (314,198))},
        {"eyes", new Implant("Глаза", "eyes", 80, "Sprites/normal_hand", new Vector2 (313,775))},
        {"ears", new Implant("Уши", "ears", 80, "Sprites/normal_hand", new Vector2 (313,758))},
        {"heart", new Implant("Сердце", "heart", 80, "Sprites/normal_hand", new Vector2 (356,539))},
        {"lungs", new Implant("Лёгкие", "lungs", 80, "Sprites/normal_hand", new Vector2 (290,506))},
        {"liver", new Implant("Печень", "liver", 80, "Sprites/normal_hand", new Vector2 (356,510))},
        {"kidneys", new Implant("Почки", "kidneys", 80, "Sprites/normal_hand", new Vector2 (320,506))}
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
            Money += 200;
            //Чем больше имплантов, тем больше зарабатываете
            HasWorkedToday = true;
            return "Вы поработали и заработали 50 монет.";
        }
        return "Вы уже работали сегодня.";
    }

    public string Sleep()
    {
        HasWorkedToday = false; //обновилли силы
        return "Вы выспались и полны сил.";
    }

    public string BuyImplant(Implant implant)
    {
        if (Money >= implant.Price)
        {
            implantsCount++;
            Money -= implant.Price;
            Debug.Log(implant.Name);
            /*Debug.Log(Implants[implant.Slot]);*/

            //Debug.Log($"Вы заменили {Implants[implant.Slot]} на {implant.Name}");
            Debug.Log(implant.Slot);
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

