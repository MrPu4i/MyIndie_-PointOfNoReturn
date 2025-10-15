using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public GameObject adds;
    public void AddsOn()
    {
        //Запустить панель с рекламой
        adds.SetActive(true);
    }

    public void OnRoomButton()
    {
        SceneManager.LoadScene("Room");
    }
}
