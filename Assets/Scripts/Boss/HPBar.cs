using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hara.GUI;
using HoaiAnh;
public class HPBar : MonoBehaviour
{
    public Slider slider;
    public TMPro.TextMeshProUGUI txtHPPro;

    BossController unit;
    UIFollowTarget uiFollow;
    
    bool autoUpdateHP = false;

    public bool IsAutoUpdateHPBar()
    {
        return autoUpdateHP;
    }

    public UIFollowTarget getUIFollow()
    {
        if (uiFollow == null)
        {
            uiFollow = GetComponent<UIFollowTarget>();
        }
        return uiFollow;
    }

    public void ResetBossHPBar()
    {
        previousMaxHP = 0f;
    }


    int countToDisableHP;
    private void Update()
    {
        if (unit == null)
        {
            countToDisableHP++;
            if (countToDisableHP > 3)
            {
                countToDisableHP = 0;
                gameObject.SetActive(false); // 3 frame after unit is null -> auto disable to clean ui
            }
        }
        else
        {
            countToDisableHP = 0;
        }

        if (autoUpdateHP)
        {
            InternalUpdateHP();
        }
    }
    public void Init(BossController unit)
    {
        this.unit = unit;
        getUIFollow()?.SetTarget(unit.transform, 0.55f, 1f, unit.OffsetHUD);
    }    
    
    public void UpdateHP(float hp_rate)
    {
        StopAllCoroutines();
        this.slider.value = hp_rate;
    }

    private float previousMaxHP = 0;

    private void InternalUpdateHP()
    {
        if (unit)
        {
            var curHP = unit.currentStats.HP;
            var maxHP = unit.initStats.HP;
            
            if (maxHP > 0)
            {
                UpdateHP(Mathf.Clamp01((float)curHP / maxHP));
            }
            else
            {
                UpdateHP(0);
            }
            
            var hpStr = Mathf.Max((int)Mathf.Round(curHP), 0).ToString();

            if (txtHPPro != null)
            {
                txtHPPro.text = hpStr;
            }
        }
    }


    public void UpdateHP()
    {
        if (autoUpdateHP == false)
            InternalUpdateHP();
    }
    
}
