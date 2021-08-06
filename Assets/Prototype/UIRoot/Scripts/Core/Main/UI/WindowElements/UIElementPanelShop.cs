using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Core.UI;
using TMPro;

public class UIElementPanelShop : UIWindowElement
{
    public ShopItem[] Items;
    public UIElementShopItem ExampleItem;

    public override void OnInitialize()
    {
        base.OnInitialize();
        CreateItems();
    }

    private void CreateItems()
    {
        foreach (var item in Items)
        {
            item.Item = Instantiate(ExampleItem, ExampleItem.transform.parent);
            item.Item.InitializeItem(item);
        }
        ExampleItem.Disable();
    }

    [System.Serializable]
    public class ShopItem
    {
        public Sprite ItemImage;
        public Action OnPressButtonBuy;
        public int Price;
        [HideInInspector] public UIElementShopItem Item;
    }
}
