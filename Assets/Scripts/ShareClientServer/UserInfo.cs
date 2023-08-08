using System;
using System.Collections;
using System.Collections.Generic;
using Hiker.Networks.Data;

public class UserInfo
{
    public long GID;

    public static readonly string[] ALL_PROPS = new string[]
    {
        "gamer",
        "inventory",
        "equipItemSlots",
        "chapter"
    };

    public static readonly string[] BATTLE_PROPS = new string[]
    {
        "gamer"
    };

    public GamerData gamer;
    public List<ChapterData> ListChapters; // index from 0
    public List<InventoryInfor> listInventory;
    public List<ItemInventoryInfor> itemEquips;
    public long ServerTimeTick;

      public void UpdateData(UserInfo other)
    {
        if (other != null)
        {
            if (other.gamer != null)
            {
                this.gamer = other.gamer;
            }
        }

        if (other.ListChapters != null)
        {
            this.ListChapters = other.ListChapters;
        }

        if (other.listInventory != null)
        {
            this.listInventory = other.listInventory;
        }

        if (other.itemEquips != null)
        {
            this.itemEquips = other.itemEquips;
        }
        
    }

    public int GetHighestChapter()
    {
        int result = ListChapters.Count - 1;
        
        if (result >= ConfigManager.GetTotalChapter())
        {
            result = ConfigManager.GetTotalChapter() - 1;
        }

        return result;
    }

    public ChapterData GetChapterData(int idx)
    {
        if (ListChapters != null && ListChapters.Count > idx)
        {
            return ListChapters[idx];
        }

        return null;
    }

    public void AddNewChapter(int idx)
    {
        if (ListChapters == null)
        {
            ListChapters = new List<ChapterData>();
        }

        if (idx >= ConfigManager.GetTotalChapter())
        {
            return;
        }

        if (ListChapters.Exists(e => e.ChapIdx == idx))
        {
            //Debug.LogError("Check logic chapter");
        }
        else
        {
            ChapterData chapterData = new ChapterData();
            chapterData.ChapIdx = idx;
            chapterData.IsComplete = false;
            chapterData.clearedTime = 100000;
            chapterData.star = 0;
            ListChapters.Add(chapterData);
            SetCurrentChapter(idx);
        }
    }

    public int GetCurrentChapter()
    {
        return gamer.CurrentChapter >= 0 ? gamer.CurrentChapter : 0;
    }

    public void SetCurrentChapter(int chapterIndex)
    {
        gamer.CurrentChapter = chapterIndex;
    }

     public void UpdateInvenEquip(TypeItem typeItem, string codename, bool equip)
    {
        InventoryInfor item = FindInventoryItem(typeItem, codename);
        item.equiped = equip;
    }
    
    public void UpdateInventory(ItemInventoryInfor itemInventory, int count)
    {
        InventoryInfor item = FindInventoryItem(itemInventory);
        if (item == null)
        {
            InventoryInfor newItem = new InventoryInfor();
            newItem.inventoryInfor = itemInventory;
            newItem.count = count;
            listInventory.Add(newItem);
        }
        else
        {
            item.count += count;
            if (item.count <= 0)
            {
                listInventory.Remove(item);
            }
        }
    }
    
    public void UpdateInventory(TypeItem typeItem, string codename, int count)
    {
        InventoryInfor item = FindInventoryItem(typeItem, codename);
        if (item == null)
        {
            InventoryInfor newItem = new InventoryInfor();
            newItem.inventoryInfor = new ItemInventoryInfor();
            newItem.inventoryInfor.typeItem = typeItem;
            newItem.inventoryInfor.codename = codename;
            newItem.count = count;
            newItem.equiped = false;
            listInventory.Add(newItem);
        }
        else
        {
            item.count += count;
            if (item.count <= 0)
            {
                listInventory.Remove(item);
            }
        }
    }
    
    public InventoryInfor FindInventoryItem(ItemInventoryInfor item)
    {
        return listInventory.Find(e =>
            e.inventoryInfor.codename.Equals(item.codename) && e.inventoryInfor.typeItem == item.typeItem);
    }

    public InventoryInfor FindInventoryItem(TypeItem typeItem, string codename)
    {
        return listInventory.Find(e =>
            e.inventoryInfor.codename.Equals(codename) && e.inventoryInfor.typeItem == typeItem);
    }
    
    public void SetUpSlotItemEquip()
    {
        itemEquips = new List<ItemInventoryInfor>();
        itemEquips.Add(new ItemInventoryInfor(null,TypeItem.Hair));
        itemEquips.Add(new ItemInventoryInfor(null, TypeItem.Hat));
        itemEquips.Add(new ItemInventoryInfor(null,TypeItem.Shoes));
        itemEquips.Add(new ItemInventoryInfor(null,TypeItem.Back));
        itemEquips.Add(new ItemInventoryInfor(null, TypeItem.Weapon));
        itemEquips.Add(new ItemInventoryInfor(null,TypeItem.Socks));
        itemEquips.Add(new ItemInventoryInfor(null, TypeItem.Cloth));
        itemEquips.Add(new ItemInventoryInfor(null, TypeItem.Eyes));
    }

    public bool CheckItemCanEquip(ItemInventoryInfor infor)
    {
        return itemEquips.Find(e => e.typeItem == infor.typeItem) != null;
    }
    
    public bool CheckItemEquiped(ItemInventoryInfor inventoryInfor)
    {
        InventoryInfor item = FindInventoryItem(inventoryInfor);
        if (item != null)
        {
            return item.equiped;
        }

        return false;
    }

    /// <summary>
    /// Tra ve item bi go ra
    /// </summary>
    /// <param name="typeItem"></param>
    /// <returns></returns>
    public void UnequipItem(TypeItem typeItem)
    {
        ItemInventoryInfor result = itemEquips.Find(e => e.typeItem == typeItem);
        if (!string.IsNullOrEmpty(result.codename))
        {
            UpdateInvenEquip(result.typeItem, result.codename, false);
            result.codename = null;
        }
    }
    
    public void EquipItem(TypeItem typeItem, string codename)
    {
        UnequipItem(typeItem);
        UpdateInvenEquip(typeItem, codename, true);
        ItemInventoryInfor itemEquip = itemEquips.Find(e => e.typeItem == typeItem);
        itemEquip.codename = codename;
    }

    public ItemInventoryInfor GetSlotItemEquip(TypeItem item)
    {
        return itemEquips.Find(e => e.typeItem == item);
    }
    
    public long GetCurrentGold()
    {
        return gamer.currentGolds;
    }

    public void RemoveItemFromInventory(TypeItem type, string codename, int countChange)
    {
        InventoryInfor userInventoryInfor = listInventory.Find(e =>
            e.inventoryInfor.typeItem == type && e.inventoryInfor.codename.Equals(codename));
        
        int newQuality = userInventoryInfor.count - countChange;
                
        int index = listInventory.IndexOf(userInventoryInfor);

        if (newQuality > 0)
        {
            listInventory[index].count = newQuality;
        }
        else
        {
            if (userInventoryInfor.equiped)
            {
                UnequipItem(type);
            }
           listInventory.RemoveAt(index);
        }
    }
}

[System.Serializable]
public class GamerData
{
    public long GID;
    public string Name;
    public int CurrentChapter;
    public long currentGolds;

    public string displayName;
    public string Token;
    public string Lang;
    public string Platform;
    public DateTime RegisterTime;
    public string LastLoginHalfDay;
    public DateTime LastLoginTime;

    public string PushToken;
    
    private void ChangeGold(long add)
    {
        this.currentGolds += add;
        if (this.currentGolds < 0) this.currentGolds = 0;
    }
    
    private void ChangeGold(long add, string mota = "")
    {

        this.currentGolds += add;
        if (this.currentGolds < 0) this.currentGolds = 0;

#if SERVER_CODE
        if (add != 0) GameServer.Database.LogUI.LogToMongo(this.GID, this.Name, "Gold", mota, this.currentGolds, add);
#endif
    }

    public void AddGold(long add, string mota = "")
    {
        if (add > 0)
        {
            ChangeGold(add, mota);
        }
    }

    public void SubGold(long add, string mota = "")
    {
        if (add > 0)
        {
            ChangeGold(-add, mota);
        }
    }

}

[System.Serializable]
public class ItemInventoryInfor
{
    public TypeItem typeItem;
    public string codename;

    public ItemInventoryInfor()
    {
        
    }

    public ItemInventoryInfor(string name, TypeItem type)
    {
        codename = name;
        typeItem = type;
    }
}

[System.Serializable]
public class InventoryInfor
{
    public ItemInventoryInfor inventoryInfor;
    public int count;
    public bool equiped;
    public InventoryInfor()
    {
        equiped = false;
    }

    public static InventoryInfor Clone(InventoryInfor infor)
    {
        InventoryInfor newInventoryInfor = new InventoryInfor();
        newInventoryInfor.inventoryInfor = new ItemInventoryInfor(infor.inventoryInfor.codename, infor.inventoryInfor.typeItem);
        newInventoryInfor.count = infor.count;
        newInventoryInfor.equiped = infor.equiped;
        return newInventoryInfor;
    }
    
    public bool CheckSameTypeItem(ItemInventoryInfor item)
    {
        return inventoryInfor.typeItem == item.typeItem && inventoryInfor.codename.Equals(item.codename);
    }
}