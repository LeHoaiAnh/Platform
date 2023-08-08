using System;
using System.Collections;
using System.Collections.Generic;
using Hara.GUI;
using HoaiAnh;
using UnityEngine;
using UnityEngine.UI;

public class PopupInventory : PopupBase
{
    public static PopupInventory Instance = null;

    [Header("Bags")]
    [SerializeField] private GameObject invenPrefab;
    [SerializeField] private Transform bagsTransform;
    private List<InventoryItemUI> listUIs = new List<InventoryItemUI>();
    
    [Header("Selected Item")]
    [SerializeField] private SelectedItem selectedItem;
    
    [Header("Item Slots")]
    [SerializeField] private ItemEquipSlots itemEquipSlots;

    [Header("Close")] [SerializeField]
    private Button closeBtn;


    private void Start()
    {
        closeBtn.onClick.AddListener(OnCloseBtnClick);
    }

    public static PopupInventory Create()
    {
        if (Instance == null)
        {
            GameObject go = PopupManager.instance.GetPopup("PopupInventory");
            Instance = go.GetComponent<PopupInventory>();
        }
        
        
        
        Instance.Init();
        return Instance;
    }

    void Init()
    {
        itemEquipSlots.ShowSlots();
        selectedItem.Reset();
        ShowBag();
    }

    void ShowBag()
    {
        listUIs.Clear();
        InventoryItemUI[] items = bagsTransform.GetComponentsInChildren<InventoryItemUI>();
        for (int i = 0; i < items.Length; i++)
        {
            ObjectPoolManager.Unspawn(items[i].gameObject);
        }
        foreach (var VARIABLE in GameClient.instance.UInfo.listInventory)
        {
            var obj = ObjectPoolManager.Spawn(invenPrefab);
            obj.transform.SetParent(bagsTransform);
            InventoryItemUI objUI = obj.GetComponent<InventoryItemUI>();
            objUI.SetStas(VARIABLE);
            listUIs.Add(objUI);
        }
    }

    public void ShowInforItem(InventoryItemUI itemUI)
    {
        selectedItem.ShowContent(itemUI);
    }

    /// <summary>
    /// when an inventory remove
    /// </summary>
    public void UpdateVisual()
    {
        itemEquipSlots.ShowSlots();
        selectedItem.Reset();
        ShowBag();
    }
}
