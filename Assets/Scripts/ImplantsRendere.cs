using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImplantsRendere : MonoBehaviour
{
    [Header("Settings")]
    public RectTransform panel; // Перетащите сюда вашу Panel из Hierarchy
    public Sprite teloSprite;
    public Sprite teloSpriteT;

    [HideInInspector] public GameObject implantUIB;
    [HideInInspector] public GameObject implantUIT;

    [Header("Implant Display")]
    public Vector2 baseImplantSize = new Vector2(50, 50); // Базовый размер импланта

    private List<GameObject> currentImplantDisplays = new List<GameObject>();
    public bool IsReady = false;

    private void Start()
    {
        Debug.Log("Начали рендерить все импланты");
        RenderAllImplants();

    }

    void Update()
    {
        // Если нужно обновлять в реальном времени
        // RefreshImplantDisplay();
    }

    public void RenderAllImplants()
    {
        // Очищаем старые импланты
        ClearImplantDisplay();

        //обычное тело
        implantUIB = new GameObject("ImplantUI тело");
        implantUIB.transform.SetParent(panel, false);
        Image image = implantUIB.AddComponent<Image>();
        image.sprite = teloSprite;
        Debug.Log(image.sprite);
        image.preserveAspect = true; // Сохраняем пропорции спрайта
        RectTransform rect = implantUIB.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(0, 0);
        rect.sizeDelta = new Vector2(620, 940);
        currentImplantDisplays.Add(implantUIB);

        implantUIT = new GameObject("ImplantUI тёмное тело");
        implantUIT.transform.SetParent(panel, false);
        Image imageT = implantUIT.AddComponent<Image>();
        imageT.sprite = teloSpriteT;
        Debug.Log(imageT.sprite);
        imageT.preserveAspect = true; // Сохраняем пропорции спрайта
        RectTransform rectT = implantUIT.GetComponent<RectTransform>();
        rectT.anchoredPosition = new Vector2(0, 0);
        rectT.sizeDelta = new Vector2(620, 940);
        currentImplantDisplays.Add(implantUIT);

        // Отрисовываем все импланты игрока
        foreach (var implant in Player.Instance.Implants.Values)
        {
            CreateImplantOnPanel(implant, implantUIB.transform, implantUIT.transform);
        }

        IsReady = true;
    }

    private void CreateImplantOnPanel(Implant implant, Transform parent1, Transform parent2)
    {

        if (implant.Sprite == null) return;

        GameObject implantUI = new GameObject("ImplantUI" + implant.Name);

        if (implant.Slot == "arms" || implant.Slot == "legs" || implant.Slot == "eyes" || implant.Slot == "ears")
        {
            implantUI.transform.SetParent(parent1, false);
        }
        else if(implant.Slot == "heart" || implant.Slot == "lungs" || implant.Slot == "liver" || implant.Slot == "kidneys")
        {
            implantUI.transform.SetParent(parent2, false);
        }

        // Добавляем Image компонент
        Image image = implantUI.AddComponent<Image>();
        image.sprite = implant.Sprite;
        image.preserveAspect = true; // Сохраняем пропорции спрайта
        RectTransform rect = implantUI.GetComponent<RectTransform>();
        // Размер
        rect.sizeDelta = new Vector2(620, 940);

        // Позиция на панели
        rect.anchoredPosition = implant.Position;

        /*// Центр в точке позиции
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchorMin = new Vector2(0, 0);
        rect.anchorMax = new Vector2(0, 0);*/

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
