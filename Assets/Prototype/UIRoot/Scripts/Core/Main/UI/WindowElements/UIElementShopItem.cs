using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.UI;
using UnityEngine.UI;
using TMPro;

public class UIElementShopItem : UIWindowElement
{
    public Image Image;
    public TextMeshProUGUI Price;
    public UIWindowElementButton Button;

    public void InitializeItem(UIElementPanelShop.ShopItem item)
    {
        Image.sprite = item.ItemImage;
        Price.text = item.Price.ToString();
        Button.OnClickEvent.AddListener(() => { item.OnPressButtonBuy?.Invoke(); });
    }

   
}
