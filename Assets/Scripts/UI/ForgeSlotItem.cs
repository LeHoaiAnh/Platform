using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForgeSlotItem : MonoBehaviour
{
    [SerializeField] public ShowItem showItem;
    [SerializeField] private ShowTypeItem showTypeItem;
    [SerializeField] private Button btn;
    
    private InventoryInfor infor;

    private void Start()
    {
        btn.onClick.AddListener(() =>
        {
            if (infor != null && PopupForge.instance != null)
            {
                PopupForge.instance.HandleRemoveItem(infor);
            }
        });
    }

    public void ShowSlot(TypeItem typeItem, ERarity rarity)
    {
        infor = null;
        showItem.gameObject.SetActive(false);
        showTypeItem.gameObject.SetActive(true);
        showTypeItem.ShowImgIcon(typeItem, rarity);
    }

    public void ShowItem(InventoryInfor item)
    {
        infor = item;
        showItem.gameObject.SetActive(true);
        showTypeItem.gameObject.SetActive(false);
        showItem.ShowImgIcon(item.inventoryInfor.typeItem, item.inventoryInfor.codename);
    }
}
