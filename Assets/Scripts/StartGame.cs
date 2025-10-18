using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public GameObject adds;
    public GameObject story;
    public GameObject text1;
    public GameObject text2;
    public GameObject text3;
    public GameObject text4;
    public GameObject text5;

    int cnt = 0;
    private void Awake()
    {
        adds.SetActive(false);
        story.SetActive(false);
    }
    public void StoryOn()
    {
        //Запустить панель с рекламой
        story.SetActive(true);
    }

    public void OnRoomButton()
    {

        SceneManager.LoadScene("Room"); //Оставить эту сцену работающей
    }

    public void NextText()
    {
        if (cnt == 0)
        {
            text1.SetActive(true);
            cnt++;
            return;
        }
        if (cnt == 1)
        {
            text2.SetActive(true);
            cnt++;
            return;
        }
        if (cnt == 2)
        {
            text3.SetActive(true);
            cnt++;
            return;
        }
        if (cnt == 3)
        {
            text4.SetActive(true);
            cnt++;
            return;
        }
        if (cnt == 4)
        {
            text5.SetActive(true);
            cnt++;
            return;
        }
        Debug.Log("Включаем рекламу");
        adds.SetActive(true);
    }
}
