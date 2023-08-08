using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPickup : MonoBehaviour
{
    public EffectConfig effectConfig;
    public bool triggerOnce = true;
    private PlayerUnit _curPlayer;
    public float interval = 1f;
    private float curInterval;
    private void OnEnable()
    {
        _curPlayer = null;
    }

    private void OnDisable()
    {
        _curPlayer = null;
    }

    void OnUnitEnter(PlayerUnit playerUnit)
    {
        if(playerUnit != null )
        {
            BattleEffect effect = new BattleEffect(effectConfig);
            playerUnit.GetStatusEff().ApplyEffect(effect);
            //ObjectPoolManager.Unspawn(gameObject);
            if (triggerOnce)
            {
                gameObject.SetActive(false);
            }
            else
            {
                _curPlayer = playerUnit;
                curInterval = 0;
            }
        }
    }

    void OnUnitExit(PlayerUnit playerUnit)
    {
        if (playerUnit != null && _curPlayer == playerUnit)
        {
            _curPlayer = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerUnit playerUnit = other.GetComponent<PlayerUnit>();
        OnUnitEnter(playerUnit);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        PlayerUnit playerUnit = col.gameObject.GetComponent<PlayerUnit>();
        OnUnitEnter(playerUnit);
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        PlayerUnit playerUnit = col.gameObject.GetComponent<PlayerUnit>();
        OnUnitExit(playerUnit);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        PlayerUnit playerUnit = other.GetComponent<PlayerUnit>();
        OnUnitExit(playerUnit);
    }

    protected virtual void Update()
    {
        if (_curPlayer != null)
        {
             if (curInterval > interval)
            {
                curInterval = 0f;
                BattleEffect effect = new BattleEffect(effectConfig);
                _curPlayer.GetStatusEff().ApplyEffect(effect);
            }
            else
            {
                curInterval += Time.deltaTime;
            }
        }
    }
}
