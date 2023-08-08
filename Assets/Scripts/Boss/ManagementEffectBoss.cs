using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HoaiAnh;
using UnityEngine;

public class ManagementEffectBoss : MonoBehaviour
{
    private List<BattleEffect> mListEffect = new List<BattleEffect>();
    private BossController bossController;
    private void Awake()
    {
        bossController = GetComponent<BossController>();
    }

    private void Update()
    {
        if (bossController == null || bossController.IsAlive() == false) return;
        EffectCycle(Time.deltaTime);
    }
    
    protected virtual void EffectCycle(float deltaTime)
    {
        for (int i = 0; i < mListEffect.Count; ++i)
        {
            if (mListEffect[i] != null) 
            {
                mListEffect[i].OnTick(deltaTime);
            }
        }

        for (int i = 0; i < mListEffect.Count; i++)
        {
            var eff = mListEffect[i];
            if (eff == null)
            {
                mListEffect.RemoveAt(i);
            }
            else if (eff.Duration <= 0)
            {
                switch (eff.Type)
                {
                    case EffectType.Heal_Hp_Boss:
                        UnapplyHealHpBoss(mListEffect[i]);
                        break;
                    default:
                        break;
                }
                mListEffect.RemoveAt(i);
            }
        }
    }

    public void ShowVisualEff(EffectType type, bool show = true)
    {
        var obj = bossController.EffectVisuals.First(e => e.type == type).obj;
        obj.SetActive(show);

    }
    public void ApplyEffect(BattleEffect effect)
    {
        if (bossController == null || bossController.IsAlive() == false) return;
        
        effect.IsActive = true;
        effect.boss = bossController;
        
        #region Buff HP Boss

        if (effect.Type == EffectType.Heal_Hp_Boss)
        {
            BattleEffect eff = mListEffect.Find(e => e.Type == effect.Type);
            // Reset Duration
            if (eff != null && eff.Duration > 0)
            {
                eff.Duration = Mathf.Max(eff.Duration, effect.Duration);
            }
            else
            {
                mListEffect.RemoveAll(e => e.Type == EffectType.Heal_Hp_Boss);
                mListEffect.Add(effect);
                ApplyHealHpBoss(effect);
            }
        }
        #endregion
    }

    private void ApplyHealHpBoss(BattleEffect effect)
    {
        ShowVisualEff(effect.Type);
        bossController.HealHpPerTime(effect.Config.Param1);
    }
    
    private void UnapplyHealHpBoss(BattleEffect effect)
    {
        ShowVisualEff(effect.Type, false);
        bossController.HealHpPerTime(0);
    }
    
    public bool IsHaveActiveEffect(EffectType effType)
    {
        return  mListEffect.Exists(e => e.Type == effType && e.IsActive && e.Duration > 0);
    }
}
