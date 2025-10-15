using UnityEngine;
using System;

[Serializable]
public class Implant
{
    public string Name;
    public string Slot; // "left_arm", "right_eye", "heart", "brain"
    public int Price;
    public Sprite Sprite; // Картинка для этой части тела
    public Vector2 Position; // Координаты на теле

    public Implant(string name, string slot, int price/*, Sprite sprite, Vector2 position*/)
    {
        //Имя, цену передадим в магазине
        Name = name;
        Slot = slot;
        Price = price;
        /*Sprite = sprite;
        Position = position;*/
    }
}
