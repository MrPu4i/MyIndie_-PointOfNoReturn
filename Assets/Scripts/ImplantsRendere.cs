using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImplantsRendere : MonoBehaviour
{
    [Header("Settings")]
    public RectTransform panel; // Перетащите сюда вашу Panel из Hierarchy

    [Header("Implant Display")]
    public Vector2 baseImplantSize = new Vector2(50, 50); // Базовый размер импланта

    private List<GameObject> currentImplantDisplays = new List<GameObject>();

    void Start()
    {
        RenderAllImplants();
    }

    void Update()
    {
        // Если нужно обновлять в реальном времени
        // RefreshImplantDisplay();
    }

    public void RenderAllImplants()
    {
        /*Debug.Log("Рендерим темплейтс");*/
        // Очищаем старые импланты
        ClearImplantDisplay();

        // Отрисовываем все импланты игрока
        foreach (var implant in Player.Instance.Implants.Values)
        {
            /*Debug.Log($"{implant.Name},{implant.Sprite}");*/
            CreateImplantOnPanel(implant);
        }
    }

    private void CreateImplantOnPanel(Implant implant)
    {

        if (implant.Sprite == null) return;

        /*Debug.Log("Создаем UI элемент для импланта");*/
        // Создаем UI элемент для импланта
        GameObject implantUI = new GameObject("ImplantUI"+implant.Name);
        implantUI.transform.SetParent(panel, false);

        // Добавляем Image компонент
        Image image = implantUI.AddComponent<Image>();
        image.sprite = implant.Sprite;
        image.preserveAspect = true; // Сохраняем пропорции спрайта

        // Настраиваем RectTransform
        RectTransform rect = implantUI.GetComponent<RectTransform>();

        // Размер
        /*rect.sizeDelta = baseImplantSize * implant.Scale;*/

        // Позиция на панели
        rect.anchoredPosition = implant.Position;

        // Центр в точке позиции
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchorMin = new Vector2(0, 0);
        rect.anchorMax = new Vector2(0, 0);

        currentImplantDisplays.Add(implantUI);
    }

    private void ClearImplantDisplay()
    {
        /*Debug.Log("Очищаем");*/
        foreach (var implantUI in currentImplantDisplays)
        {
            if (implantUI != null)
                Destroy(implantUI);
        }
        currentImplantDisplays.Clear();
    }

    // Метод для обновления отображения (если импланты меняются динамически)
    public void RefreshImplantDisplay()
    {
        RenderAllImplants();
    }
}
