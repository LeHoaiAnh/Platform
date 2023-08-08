using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class EffectConfig
{
    public EffectType Type;
    public float Duration;
    public long Damage;
    public float Param1;
    public float Param2;
    public float Param3;

    public EffectConfig Clone()
    {
        return new EffectConfig()
        {
            Type = this.Type,
            Duration = this.Duration,
            Damage = this.Damage,
            Param1 = this.Param1,
            Param2 = this.Param2,
            Param3 = this.Param3,
        };
    }
}

public enum EffectType
{
    
    #region Player
    BuffEnergy,
    Shield,
    BuffSpeed,
    PureDamage,
    #endregion
    
    #region Boss
    Heal_Hp_Boss
    #endregion
}

