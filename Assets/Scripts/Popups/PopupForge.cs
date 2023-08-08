using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Hara.GUI;
using Hiker.Networks.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PopupForge : PopupBase
{
    public static PopupForge instance;
    [Header ("Right Pannel")]
    [SerializeField] private GameObject forgeItem;
    [SerializeField] private Transform itemContainer;
    private List<ForgeItem> listForgeItems;
    
    [Header("Left Pannel")] [SerializeField]
    private ForgeSlotItem[] items;
    private List<ForgeMaterial> forgeInfors;
    [SerializeField] private Button forgeBtn;
    [SerializeField] private ShowTypeItem showUpgradeItem;
    
    [Header ("Close")] [SerializeField]
    private Button closeBtn;

    private void Start()
    {
        closeBtn.onClick.AddListener(() => OnCloseBtnClick(true));
        forgeBtn.onClick.AddListener(Forge);
    }

    private void Forge()
    {
        GameClient.instance.RequestForge(forgeInfors);
    }

    public static PopupForge Create()
    {
        if (instance)
        {
            Destroy(instance.gameObject);
            instance = null;
        }

        var go = PopupManager.instance.GetPopup("PopupForge");
        instance = go.GetComponent<PopupForge>();
        instance.Init();
        return instance;
    }
    
    public void Init()
    {
        Reset();
        LoadAllItem();
    }
    
    void Reset()
    {
        if (forgeInfors != null)
        {
            forgeInfors.Clear();
        }
        else
        {
            forgeInfors = new List<ForgeMaterial>();
        }

        if (listForgeItems != null)
        {
            listForgeItems.Clear();
        }
        else
        {
            listForgeItems = new List<ForgeItem>();
        }

        for (int i = 0; i < items.Length; i++)
        {
            items[i].gameObject.SetActive(false);
        }
        showUpgradeItem.gameObject.SetActive(false);
        forgeBtn.gameObject.SetActive(false);
    }

    private void LoadAllItem()
    {
        ForgeItem[] old = itemContainer.GetComponentsInChildren<ForgeItem>();
        for (int i = 0; i < old.Length; i++)
        {
            SimplePool.Despawn(old[i].gameObject);
        }
        for (int i = 0; i < GameClient.instance.UInfo.listInventory.Count; i++)
        {
            ItemStatsInfor statsInfor = ConfigManager.itemStatsCfg.GetItemCfg(GameClient.instance.UInfo.listInventory[i].inventoryInfor.typeItem, GameClient.instance.UInfo.listInventory[i].inventoryInfor.codename);
            if(statsInfor.Rarity != ERarity.Legend)
            {
                var obj = SimplePool.Spawn(forgeItem, itemContainer);
                var forgeObj = obj.GetComponent<ForgeItem>();
                forgeObj.Show(GameClient.instance.UInfo.listInventory[i]);
                listForgeItems.Add(forgeObj);
            }
        }
    }

    ForgeMaterial FindForgeMaterials(TypeItem type, string codename)
    {
        return forgeInfors.Find(e => e.infor.codename.Equals(codename) && e.infor.typeItem == type);
    }
    public void HandleSelectItem(InventoryInfor infor)
    {
        if (forgeInfors.Sum(e => e.count) >= 4)
        {
            PopupMessage.Create(MessagePopupType.TEXT, "Enough items for forging");
            return;
        }
        ForgeMaterial forgeMaterial = FindForgeMaterials(infor.inventoryInfor.typeItem, infor.inventoryInfor.codename);
        //Add more
        if (forgeMaterial != null)
        {
            forgeMaterial.count += 1;
            ShowPreview();
        }
        //Add
        else
        {
            ForgeMaterial material = new ForgeMaterial();
            material.count = 1;
            material.infor = new ItemInventoryInfor(infor.inventoryInfor.codename, infor.inventoryInfor.typeItem);
            forgeInfors.Add(material);
            ShowPreview();
        }
        
        forgeBtn.gameObject.SetActive(forgeInfors.Sum(e => e.count) == 4);
    }

    public void HandleRemoveItem(InventoryInfor infor)
    {
        ForgeMaterial forgeMaterial = FindForgeMaterials(infor.inventoryInfor.typeItem, infor.inventoryInfor.codename);
        if (forgeMaterial != null)
        {
            forgeMaterial.count -= 1;
            if (forgeMaterial.count <= 0)
            {
                forgeInfors.Remove(forgeMaterial);
            }
            ShowPreview();
            forgeBtn.gameObject.SetActive(false);
        }
    }
    
    public void ShowPreview()
    {
        if (forgeInfors.Count == 0)
        {
            for (int i = 0; i < items.Length; i++)
            {
                items[i].gameObject.SetActive(false);
            }

            for (int i = 0; i < listForgeItems.Count; i++)
            {
                listForgeItems[i].Reset();
            }
            showUpgradeItem.gameObject.SetActive(false);
        }
        else
        {
            int index = 0;
            ItemStatsInfor statsInfor = ConfigManager.itemStatsCfg.GetItemCfg(forgeInfors[0].infor.typeItem, forgeInfors[0].infor.codename);

            for (int i = 0; i < forgeInfors.Count; i++)
            {
                for (int j = 0; j < forgeInfors[i].count; j++)
                {
                    items[index].gameObject.SetActive(true);
                    items[index].ShowItem(GameClient.instance.UInfo.FindInventoryItem(forgeInfors[i].infor));
                    index += 1;
                }
                
            }

            for (int i = index; i < 4; i++)
            {
                items[i].gameObject.SetActive(true);
                items[i].ShowSlot(forgeInfors[0].infor.typeItem, statsInfor.Rarity);
            }
            
            for (int i = 0; i < listForgeItems.Count; i++)
            {
                listForgeItems[i].Reset();
                ForgeMaterial forgeMaterial = FindForgeMaterials(listForgeItems[i].inventoryInfor.inventoryInfor.typeItem,
                    listForgeItems[i].inventoryInfor.inventoryInfor.codename);
                if (forgeMaterial != null)
                {
                    listForgeItems[i].ChangeQualityText(forgeMaterial.count);
                }
                else
                {
                    if (!CheckSameType(listForgeItems[i], forgeInfors[0].infor.typeItem,
                            items[0].showItem.statsInfor.Rarity))
                    {
                        listForgeItems[i].ShowLock();
                    }
                }
            }
            
            showUpgradeItem.gameObject.SetActive(true);
            showUpgradeItem.ShowImgIcon(forgeInfors[0].infor.typeItem,
               (ERarity) (items[0].showItem.statsInfor.Rarity + 1));
        }
    }

    private bool CheckSameType(ForgeItem item, TypeItem type, ERarity rarity)
    {
        return item.inventoryInfor.inventoryInfor.typeItem == type
               && item.statsInfor.Rarity == rarity;
    }
    
}
