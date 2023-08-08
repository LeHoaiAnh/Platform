using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEffect
{
    public EffectConfig Config;
    public float Duration { get; set; }

    float mTimeInterval = 0;
    public bool IsActive { get; set; }

    public PlayerUnit player { get; set; }
    public BossController boss { get; set; }

    public EffectType Type { get { return Config.Type; } }

    public BattleEffect(EffectConfig cfg)
    {
        Config = cfg.Clone();
        Duration = cfg.Duration;
        mTimeInterval = 0;
    }

    public void OnTick(float deltaTime)
    {
        if (IsActive)
        {
            Duration -= deltaTime;
            mTimeInterval += deltaTime;

            if (player != null && player.IsAlive())
            {
                switch (Type)
                {
                    default:
                        break;
                }
            }

            if (boss != null && boss.IsAlive())
            {
                switch (Type)
                {
                    default:
                        break;
                }
            }
        }
    }
}
