using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectedItem : MonoBehaviour
{
    [Header("Empty")] [SerializeField]
    private GameObject emptyObj;

    [Header("Content")] [SerializeField]
    private GameObject contentObj;
    [SerializeField] private TextMeshProUGUI Name;
    [SerializeField] private TextMeshProUGUI Description;
    [SerializeField] private ShowIconItem Icon;
    [SerializeField] private Image Bg;
    private InventoryItemUI itemUI;
    private InventoryInfor inventoryInfor;
    private ItemStatsInfor statsInfor;
    
    [Header("Btn Layouts")] 
    [SerializeField]private Button equip;
    [SerializeField]private Button unEquip;
    [SerializeField]private Button sell;

    private void Start()
    {
        equip.onClick.AddListener(Equip);
        unEquip.onClick.AddListener(UnEquip);
        sell.onClick.AddListener(Sell);
    }

    private void Sell()
    {
        
    }

    private void UnEquip()
    {
        GameClient.instance.RequestEquipItem(inventoryInfor.inventoryInfor.typeItem, inventoryInfor.inventoryInfor.codename, false);
    }

    private void Equip()
    {
        GameClient.instance.RequestEquipItem(inventoryInfor.inventoryInfor.typeItem, inventoryInfor.inventoryInfor.codename, true);
    }
    
    

    public void Reset()
    {
        emptyObj.SetActive(true);
        contentObj.SetActive(false);
    }

    public void ShowContent(InventoryItemUI itemUI)
    {
        this.itemUI = itemUI;
        inventoryInfor = itemUI.inventoryInfor;
        statsInfor = itemUI.statsInfor;
        emptyObj.SetActive(false);
        contentObj.SetActive(true);
        Icon.ShowImgIcon(inventoryInfor.inventoryInfor.typeItem, GameClient.instance.spriteCollection.GetSprite(inventoryInfor.inventoryInfor.typeItem + "_" + inventoryInfor.inventoryInfor.codename));
        Bg.sprite = GameClient.instance.spriteCollection.GetSprite("Frame_" + statsInfor.Rarity);
        Name.text = Localization.Get(inventoryInfor.inventoryInfor.codename + "_" + "Name");
        Description.text = String.Format(Localization.Get(inventoryInfor.inventoryInfor.codename + "_" + "Description"), statsInfor.Stats[0].Val);

        if (GameClient.instance.UInfo.CheckItemCanEquip(inventoryInfor.inventoryInfor))
        {
            if (GameClient.instance.UInfo.CheckItemEquiped(inventoryInfor.inventoryInfor))
            {
                unEquip.gameObject.SetActive(true);
                equip.gameObject.SetActive(false);
            }
            else
            {
                unEquip.gameObject.SetActive(false);
                equip.gameObject.SetActive(true);
            }
        }
        else
        {
            equip.gameObject.SetActive(false);
            unEquip.gameObject.SetActive(false);
        }
    }
}
