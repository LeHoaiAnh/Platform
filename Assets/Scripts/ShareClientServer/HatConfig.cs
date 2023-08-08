using System.Collections;
using System.Collections.Generic;

public enum EStatModType
{
    ADD,
    MUL, // Multiply or scale
    BUFF, // Increase by percent
}
public enum EStatType
{
    #region Basic Stats
    HP,                                 
    ATK,
    Armor,
    Speed
    #endregion
}

public enum Genre
{
    Male,
    Female,
    Both
}

public class StatMod
{
    public EStatType Stat;
    public EStatModType Mod;
    public double Val;
    public double[] Inc;
}

public enum ERarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legend
}

public class ItemStatsInfor
{
    public string Codename;
    public Genre Genre;
    public ERarity Rarity;
    public StatMod[] Stats;
}

public class ItemTypeStatsCfg
{
    public TypeItem typeItem;
    public List<ItemStatsInfor> itemStatsCfgs;
}

public class ItemStatsCfg
{
    public List<ItemTypeStatsCfg> itemInfors;

    public ItemStatsInfor GetItemCfg(TypeItem type, string codename)
    {
        return itemInfors.Find(e => e.typeItem == type).itemStatsCfgs.Find(e => e.Codename.Equals(codename));
    }
    
    public List<ItemStatsInfor> GetAllItemByRarityType(TypeItem type, ERarity rarity)
    {
        return itemInfors.Find(e => e.typeItem == type).itemStatsCfgs.FindAll(e => e.Rarity == rarity);
    }
}