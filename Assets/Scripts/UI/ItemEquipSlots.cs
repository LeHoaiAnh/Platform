using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HoaiAnh;
using UnityEngine;

public class ItemEquipSlots : MonoBehaviour
{
    private InventoryItemSlot[] allSlots;
    
    private void OnEnable()
    {
        allSlots = HoaiAnh.Util.ArrayPool<InventoryItemSlot>.Claim(16);
        allSlots = GetComponentsInChildren<InventoryItemSlot>();
    }

    private void OnDisable()
    {
        HoaiAnh.Util.ArrayPool<InventoryItemSlot>.Release(ref allSlots);
    }

    public void ShowSlots()
    {
        for (int i = 0; i < allSlots.Length; i++)
        {
            allSlots[i].Show();
        }
    }

    public void ChangeInforType(TypeItem typeItem)
    {
        allSlots.First(e=> e.type == typeItem).Show();
    }
}
