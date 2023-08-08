using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HoaiAnh;
using UnityEngine;

[Serializable]
public class EffectVisual
{
    public EffectType type;
    public GameObject obj;
}
public class ManagementEffect : MonoBehaviour
{
    private List<BattleEffect> mListEffect = new List<BattleEffect>();
    private PlayerUnit player;
    private void Awake()
    {
        player = GetComponent<PlayerUnit>();
    }

    private void Update()
    {
        if (player == null || player.IsAlive() == false) return;
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
                    case EffectType.Shield:
                        UnapplyShielEff();
                        break;
                    case EffectType.BuffSpeed:
                        UnapplySpeedUpEff(mListEffect[i]);
                        break;
                    default:
                        break;
                }
                mListEffect.RemoveAt(i);
            }
        }
    }

    public void UnapplyShielEff()
    {
        ShowVisualEff(EffectType.Shield, false);
    }
    public void ShowVisualEff(EffectType type, bool show = true)
    {
        var obj = player.EffectVisuals.First(e => e.type == type).obj;
        obj.SetActive(show);

    }
    public void ApplyEffect(BattleEffect effect)
    {
        if (player == null || player.IsAlive() == false) return;
        
        effect.IsActive = true;
        effect.player = player;

        #region Buff Energy
        if (effect.Type == EffectType.BuffEnergy)
        {
            player.GetEnergy(effect.Config.Param1);
            var obj = player.EffectVisuals.First(e => e.type == effect.Type).obj;
            obj.SetActive(true);
            Utils.DoAction(this, () =>
            {
                obj.SetActive(false);
            }, 1f, true);
        }
        #endregion

        #region Buff SHIED

        if (effect.Type == EffectType.Shield)
        {
            ShowVisualEff(effect.Type);
            BattleEffect eff = mListEffect.Find(e => e.Type == effect.Type);
            // Reset Duration
            if (eff != null && eff.Duration > 0)
            {
                eff.Duration = Mathf.Max(eff.Duration, effect.Duration);
            }
            else
            {
                mListEffect.RemoveAll(e => e.Type == EffectType.Shield);
                mListEffect.Add(effect);
            }
        }
        #endregion

        #region Buff Speed

        if (effect.Type == EffectType.BuffSpeed)
        {
            BattleEffect eff = mListEffect.Find(e => e.Type == effect.Type);
            // Reset Duration
            if (eff != null && eff.Duration > 0)
            {
                eff.Duration = Mathf.Max(eff.Duration, effect.Duration);
                eff.Config.Param1 = Mathf.Max(eff.Config.Param1, effect.Config.Param1);
                player.UpdateSpeed(-eff.Config.Param1);
                ApplySpeedUpEff(eff);
            }
            else
            {
                mListEffect.RemoveAll(e => e.Type == EffectType.BuffSpeed);
                mListEffect.Add(effect);
                ApplySpeedUpEff(effect);
            }
        }
        #endregion
        
        #region Pure Damage

        if (effect.Type == EffectType.PureDamage)
        {
            player.GetEnergy(- effect.Config.Damage * QuanLyManChoi.Instance.damageMultiplier);
        }
        #endregion
       
    }

    private void ApplySpeedUpEff(BattleEffect effect)
    {
        ShowVisualEff(effect.Type);
        player.UpdateSpeed(effect.Config.Param1);
    }
    
    private void UnapplySpeedUpEff(BattleEffect effect)
    {
        ShowVisualEff(effect.Type, false);
        player.UpdateSpeed(-effect.Config.Param1);
    }
    
    public bool IsHaveActiveEffect(EffectType effType)
    {
        return  mListEffect.Exists(e => e.Type == effType && e.IsActive && e.Duration > 0);
    }
}
