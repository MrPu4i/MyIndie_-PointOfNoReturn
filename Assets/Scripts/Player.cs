using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

    public int Money = 100;
    public int implantsCount = 0;
    public Dictionary<string, Implant> Implants = new Dictionary<string, Implant>()
    {
        {"arms", null},
        {"legs", null},
        {"eyes", null},
        {"ears", null},
        {"heart", null},
        {"lungs", null}
    };
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

        Instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log("Player создан и сохранен!");
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
            Debug.Log(Implants[implant.Slot]);

//Debug.Log($"Вы заменили {Implants[implant.Slot]} на {implant.Name}");
            Implants[implant.Slot] = implant; //заменяем имплант

            UpdateBodyVisual(implant.Slot);

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

    private void UpdateBodyVisual(string slot)
    {
        Debug.Log("Меняем спрайт части тела");
        /*switch (slot)
        {
            case "left_arm":
                leftArmSprite.sprite = Implants[slot].Sprite;
                break;
            case "right_arm":
                rightArmSprite.sprite = Implants[slot].Sprite;
                break;
                // и т.д.
        }*/
    }
}
