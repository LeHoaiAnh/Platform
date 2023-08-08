using System.Collections;
using System.Collections.Generic;

public enum TypeItem
{
    Hair,
    Hat,
    Shoes,
    Back,
    Weapon,
    Socks,
    Cloth,
    Eyes
}

public class ItemCfg
{
    public List<ItemTypeCfg> listItems;

    public ItemTypeCfg GetListItemByType(TypeItem typeItem)
    {
        return listItems.Find(e => e.typeItem == typeItem);
    }

    public long GetPriceItem(TypeItem typeItem, string codename)
    {
        return listItems.Find(e => e.typeItem == typeItem).items.Find(e => e.codename.Equals(codename)).price;
    }
}

public class ItemTypeCfg
{
    public TypeItem typeItem;
    public List<ItemInforCfg> items;
}
public class ItemInforCfg
{
    public string codename;
    public long price;
}
