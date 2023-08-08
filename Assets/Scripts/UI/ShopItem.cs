using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI name;
    [SerializeField] private Image bg;
    [SerializeField] private TextMeshProUGUI price;
    [SerializeField] private ShowIconItem showIconItem;
    [SerializeField] private Button buyBtn;
    private TypeItem type;
    private ItemInforCfg inforCfg;
    public ItemStatsInfor statsInfor{ get; set; }

    private void Start()
    {
        buyBtn.onClick.AddListener(() =>
        {
            PopupBuy.Create(type, inforCfg);
        });
    }

    public void SetStats(TypeItem typeItem, ItemInforCfg infor)
    {
        type = typeItem;
        inforCfg = infor;
        statsInfor = ConfigManager.itemStatsCfg.GetItemCfg(typeItem, infor.codename);
        showIconItem.ShowImgIcon(typeItem, GameClient.instance.spriteCollection.GetSprite(typeItem + "_" + infor.codename));
        name.text = Localization.Get(infor.codename + "_" + "Name");
        price.text = infor.price.ToString();
        bg.sprite = GameClient.instance.spriteCollection.GetSprite("Frame_" + statsInfor.Rarity);
    }
}
