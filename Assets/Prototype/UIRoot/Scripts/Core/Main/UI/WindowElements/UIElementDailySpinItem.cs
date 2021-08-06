using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Core.UI;
using TMPro;

public class UIElementDailySpinItem : UIWindowElement
{
    public SpinItemType Item;
    public Image ItemImage;
    public TextMeshProUGUI RewardText;
    public int Reward;
}

public enum SpinItemType
{
    Gold,
    Gem,
    Rare
}
