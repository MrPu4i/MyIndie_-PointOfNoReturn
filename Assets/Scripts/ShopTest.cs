using UnityEngine;

public class ShopTest : MonoBehaviour
{
        void Start()
        {
            Debug.Log("GameManager: " + (GameManager.Instance != null ? "Есть!" : "Нет!"));
            Debug.Log("Player: " + (Player.Instance != null ? "Есть!" : "Нет!"));
            Debug.Log("Деньги: " + Player.Instance.Money);
        }
   
}
