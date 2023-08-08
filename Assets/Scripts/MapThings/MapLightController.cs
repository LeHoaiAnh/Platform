using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public enum TypeLight
{
    HEALING_LIGHT,
    NEUTRAL_LIGHT,
    DAMAGING_LIGHT,
    NUMS_OF_TYPES
}
[RequireComponent(typeof(Light2D))]
public class MapLightController : MonoBehaviour
{
    [Header("Light type Setting")] [SerializeField]
    protected TypeLight typeOfLight;
    
    protected Light2D curLight;
    protected PlayerUnit player;
    protected virtual void Start()
    {
        curLight = GetComponent<Light2D>();
        SetLightColor();
    }

    protected  virtual void OnEnable()
    {
        player = null;
    }

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        PlayerUnit playerUnit = col.GetComponent<PlayerUnit>();
        if (playerUnit != null)
        {
            player = playerUnit;
            OnTriggerEnterHelper(playerUnit);
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D col)
    {
        PlayerUnit playerUnit = col.GetComponent<PlayerUnit>();
        if (playerUnit != null && playerUnit == player)
        {
           playerUnit.ResetConsumeSpeed();
           playerUnit.LightController.LeaveLight();
           player = null;
        }
    }

    protected void SetLightColor()
    {
        if (typeOfLight == TypeLight.HEALING_LIGHT)
        {
            curLight.color = Color.green;
        }
        else if (typeOfLight == TypeLight.NEUTRAL_LIGHT)
        {
            curLight.color = Color.white;
        }
        else
        {
            curLight.color = Color.red;
        }
    }

    protected void OnTriggerEnterHelper(PlayerUnit playerUnit)
    {
        playerUnit.UpdateConsumeSpeed(typeOfLight);
        EnterLightHelper(playerUnit.LightController);
    }

    
    protected void EnterLightHelper(PlayerLightController con)
    {
        if (typeOfLight == TypeLight.HEALING_LIGHT)
        {
            con.EnterHealingLight();
        } 
        else if (typeOfLight == TypeLight.NEUTRAL_LIGHT)
        {
            con.EnterNeutralLight();
        }
        else
        {
            con.EnterDamagingLight();
        }
    }
}
