using System;
using System.Collections;
using System.Collections.Generic;
using Hara.GUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupBuy : PopupBase
{
    static PopupBuy instance = null;
    [SerializeField] private TextMeshProUGUI content;
    [SerializeField] private TextMeshProUGUI qualityText;
    [SerializeField] private TextMeshProUGUI totalPrice;
    [SerializeField] private Button buyBtn;
    [SerializeField] private Button increase;
    [SerializeField] private Button decrease;
    [SerializeField] private Button closeBtn;
    [SerializeField] private Image bg;
    [SerializeField] private ShowIconItem showIconItem;
    
    private int quality;
    private TypeItem type;
    private ItemInforCfg inforCfg;
    
    public static PopupBuy Create(TypeItem typeItem, ItemInforCfg infor)
    {
        if (instance)
        {
            Destroy(instance.gameObject);
            instance = null;
        }

        var go = PopupManager.instance.GetPopup("PopupBuy");
        instance = go.GetComponent<PopupBuy>();
        instance.Init(typeItem, infor);
        return instance;
    }

    private void Start()
    {
        increase.onClick.AddListener(() => ChangeQuality(true));
        decrease.onClick.AddListener(() => ChangeQuality(false));
        buyBtn.onClick.AddListener(Buy);
        closeBtn.onClick.AddListener(() => OnCloseBtnClick(true));
    }

    private void Buy()
    {
        long totalPrice = quality * inforCfg.price;
        if (totalPrice > GameClient.instance.UInfo.GetCurrentGold())
        {
            PopupMessage.Create(MessagePopupType.TEXT, "You have not enough money to buy");
        }
        else
        {
            GameClient.instance.RequestBuyInventory(type, inforCfg.codename, quality);
            OnCloseBtnClick(true);
        }
    }

    private void Init(TypeItem typeItem, ItemInforCfg infor)
    {
        quality = 1;
        type = typeItem;
        inforCfg = infor;
        ShowText();
        ItemStatsInfor statsInfor = ConfigManager.itemStatsCfg.GetItemCfg(typeItem, infor.codename);
        showIconItem.ShowImgIcon(typeItem, GameClient.instance.spriteCollection.GetSprite(typeItem + "_" + infor.codename));
        bg.sprite = GameClient.instance.spriteCollection.GetSprite("Frame_" + statsInfor.Rarity);
    }

    private void ShowText()
    {
        content.text = String.Format("You confirm to buy {0} item(s) with the price {1}{2}", quality, inforCfg.price, ConfigManager.GetIconTMP_Sprite("Gold"));
        qualityText.text = quality.ToString();
        totalPrice.text = (quality * inforCfg.price).ToString();
    }

    public void ChangeQuality(bool isIncrease)
    {
        if (isIncrease)
        {
            quality += 1;
        }
        else
        {
            quality -= 1;
            quality = Math.Max(0, quality);
        }
        ShowText();
    }
    
    
}
