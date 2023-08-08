using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ForgeItem : MonoBehaviour
{
    [SerializeField] private ShowItem showItem;
    public InventoryInfor inventoryInfor { get; set; }
    public ItemStatsInfor statsInfor{ get; set; }

    [SerializeField] private TextMeshProUGUI Count;
    [SerializeField] private Button btn;
    [SerializeField] private GameObject lockObj;
    [SerializeField] private GameObject selectedObj;
    
    private void Start()
    {
        btn.onClick.AddListener(() =>
        {
            if (!lockObj.activeInHierarchy && !selectedObj.activeInHierarchy &&  PopupForge.instance != null)
            {
                PopupForge.instance.HandleSelectItem(inventoryInfor);
            }
        });
    }

    public void Show(InventoryInfor item)
    {
        lockObj.SetActive(false);
        selectedObj.SetActive(false);

        inventoryInfor = InventoryInfor.Clone(item);
        statsInfor = ConfigManager.itemStatsCfg.GetItemCfg(item.inventoryInfor.typeItem, item.inventoryInfor.codename);
        showItem.ShowImgIcon(item.inventoryInfor.typeItem, item.inventoryInfor.codename);
        Count.text = item.count.ToString();
    }

    public void ChangeQualityText(int numUsed)
    {
        int remain = inventoryInfor.count - numUsed;
        inventoryInfor.count = Math.Max(0, inventoryInfor.count);
        if (remain == 0)
        {
            ShowSelected();
        }
        
        
        Count.text = remain.ToString();

    }
    public void ShowLock()
    {
        lockObj.SetActive(true);
    }

    public void ShowSelected()
    {
        selectedObj.SetActive(true);
    }

    public void Reset()
    {
        lockObj.SetActive(false);
        selectedObj.SetActive(false);
        Count.text = inventoryInfor.count.ToString();
    }
}
