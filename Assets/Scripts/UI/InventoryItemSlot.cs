using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ItemDragReceiver))]
public class InventoryItemSlot : MonoBehaviour
{
    public TypeItem type;
    [SerializeField] private GameObject empty;
    [SerializeField] private GameObject hasItem;
    
    [Header ("Has Item Content")]
    [SerializeField] private ShowIconItem Icon;
    [SerializeField] private Image Bg;
    
    public void Show()
    {
        ItemInventoryInfor item = GameClient.instance.UInfo.GetSlotItemEquip(type);

        if (string.IsNullOrEmpty(item.codename))
        {
            empty.SetActive(true);
            hasItem.SetActive(false);
        }
        else
        {
            hasItem.SetActive(true);
            empty.SetActive(false);
            ItemStatsInfor statsInfor = ConfigManager.itemStatsCfg.GetItemCfg(item.typeItem, item.codename);
            Icon.ShowImgIcon(type, GameClient.instance.spriteCollection.GetSprite(item.typeItem + "_" + item.codename));
            Bg.sprite = GameClient.instance.spriteCollection.GetSprite("Frame_" + statsInfor.Rarity);
        }
    }
}
