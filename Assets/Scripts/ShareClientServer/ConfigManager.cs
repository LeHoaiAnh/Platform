using System.Collections;
using System.Collections.Generic;
using Hiker.Networks.Data;
using LitJson;

public partial class ConfigManager
{
    public static ConfigManager instance;
    public static PlayerStat playerCfg { get; private set; }
   public static ItemCfg itemCfg { get; private set; }
   public static ItemStatsCfg itemStatsCfg { get; private set; }
   public static JsonData otherConfig { get; private set; }

   public static bool loaded;
   
   public static void LoadConfigs()
   {
       playerCfg = ReadConfig<PlayerStat>("PlayerConfig");
       itemCfg = ReadConfig<ItemCfg>("ItemCfg");
       itemStatsCfg = ReadConfig<ItemStatsCfg>("ItemStatsCfg");
       otherConfig = ReadConfig("OtherConfig");
       loaded = true;
   }

    public static T ReadConfig<T>(string configFileName, string path = null)
    {
        string text = ReadConfigString(configFileName, path);
        if (string.IsNullOrEmpty(text)) return default(T);
        return JsonMapper.ToObject<T>(text);
    }
    
    
    
    public static JsonData ReadConfig(string configFileName, string path = null)
    {
        string text = ReadConfigString(configFileName, path);
        if (string.IsNullOrEmpty(text)) return null;
        return JsonMapper.ToObject(text);
    }
    
    public static float GetLightEnergySpeed()
    {
        if (otherConfig.Contains("LightEnergySpeed"))
        {
            float sp = (float)otherConfig["LightEnergySpeed"];
            return sp;
        }
        else
        {
            return 0;
        }
    }
    
    public static int GetTotalChapter()
    {
        if (otherConfig.Contains("TotalChapter"))
        {
            int total = (int)otherConfig["TotalChapter"];
            return total;
        }
        else
        {
            return 0;
        }
    }
    
    public static int GetFakeTotalChapter()
    {
        if (otherConfig.Contains("FakerTotalChapter"))
        {
            int total = (int)otherConfig["FakeTotalChapter"];
            return total;
        }
        else
        {
            return 0;
        }
    }
    
    public static int GetMaxHUDQueue()
    {
        if (otherConfig.Contains("MaxHUDQueue"))
        {
            int hp = (int)otherConfig["MaxHUDQueue"];
            return hp;
        }
        else
        {
            return 10;
        }
    }
    
    /// <summary>
    ///  return real value change
    /// </summary>
    /// <param name="before"></param>
    /// <param name="type"></param>
    /// <param name="valueChange"></param>
    /// <returns></returns>
    public static double CalculateStats(double before, EStatModType type, double valueChange)
    {
        switch (type)
        {
            case EStatModType.ADD:
                return valueChange;
            case EStatModType.MUL:
                return before * ( valueChange - 1);
            case EStatModType.BUFF:
                return before * ( valueChange / 100f);
            default:
                return before;
        }
    }

}
