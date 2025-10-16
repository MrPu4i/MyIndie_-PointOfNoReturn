using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopImplant : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private int implantIndex;
    [SerializeField] private GameObject implantItem; // Перетащите сюда ваш GameObject импланта\

    private Button but;

    private Shop shop;

    void Start()
    {
        shop = FindFirstObjectByType<Shop>();
        but = GetComponent<Button>();
        but.onClick.AddListener(OnButtonClick);
    }

        private void OnButtonClick()
        {
            if (shop != null)
            {
                shop.BuyImplant(implantIndex, implantItem);
            }
        }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(but.enabled == true)
            ButtonSoundsManager.Instance?.PlayHoverEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (but.enabled == true)
            ButtonSoundsManager.Instance?.PlayHoverEnter();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (but.enabled == true)
            ButtonSoundsManager.Instance?.PlayClick();
    }
}
