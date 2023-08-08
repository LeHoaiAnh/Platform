using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossDynamicLightController : MapLightController
{
    private List<BossController> bossControllers = new List<BossController>();
    public EffectConfig effectConfig;

    protected override void OnEnable()
    {
        base.OnEnable();
        bossControllers.Clear();
    }

    public void UpdateColor(TypeLight typeLight)
    {
        typeOfLight = typeLight;
        SetLightColor();
        if (player != null)
        {
            OnTriggerEnterHelper(player);
        }

        if (typeLight == TypeLight.DAMAGING_LIGHT)
        {
            for (int i = 0; i < bossControllers.Count; i++)
            {
                OnTriggerEnterHelper(bossControllers[i]);
            }
        }
    }

    protected override void OnTriggerEnter2D(Collider2D col)
    {
        base.OnTriggerEnter2D(col);

        BossController boss = col.GetComponent<BossController>();
        if (boss != null)
        {
            if (!bossControllers.Contains(boss))
            {
                bossControllers.Add(boss);
                OnTriggerEnterHelper(boss);
            }
        }
    }

    protected override void OnTriggerExit2D(Collider2D col)
    {
        base.OnTriggerExit2D(col);
        
        BossController boss = col.GetComponent<BossController>();
        if (boss != null)
        {
            if (bossControllers.Contains(boss))
            {
                bossControllers.Remove(boss);
            }
        }
    }


    protected void OnTriggerEnterHelper(BossController boss)
    {
        if (typeOfLight == TypeLight.DAMAGING_LIGHT)
        {
            BattleEffect effect = new BattleEffect(effectConfig);
            boss.GetStatusEff().ApplyEffect(effect);
        }
    }
}
