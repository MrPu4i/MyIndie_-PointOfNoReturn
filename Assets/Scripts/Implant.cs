using UnityEngine;
using System;
using UnityEngine.U2D;
using UnityEngine.UIElements;

[Serializable]
public class Implant
{
    public string Name;
    public string Slot; // "left_arm", "right_eye", "heart", "brain"
    public int Price;
    public string SpritePath; // Храним путь, а не Sprite
    public Vector2 Position; // Координаты на теле
    public bool IsSold;

    // Свойство, которое загружает спрайт только когда он нужен
    public Sprite Sprite
    {
        get
        {
            if (_sprite == null && !string.IsNullOrEmpty(SpritePath))
            {
                _sprite = Resources.Load<Sprite>(SpritePath);
            }
            return _sprite;
        }
    }
    private Sprite _sprite;

    public Implant(string name, string slot, int price, string spritePath, Vector2 position, bool isSold = false)
    {
        //Имя, цену передадим в магазине
        Name = name;
        Slot = slot;
        Price = price;
        Position = position;
        SpritePath = spritePath; // Сохраняем путь
        IsSold = isSold;
    }
}
