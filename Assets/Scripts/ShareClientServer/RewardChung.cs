using System.Collections;
using System.Collections.Generic;

public class RewardChung
{
    
}

public class ItemReward : RewardChung
{
    public string codename;
    public TypeItem type;
    public int quality;
}

public class GeneralReward : RewardChung
{
    public ItemReward[] items;
}
