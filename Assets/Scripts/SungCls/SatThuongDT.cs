using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class SatThuongDT : MonoBehaviourPun
{
    [SerializeField] protected float lifeTimeDuration = 10f;
    public List<EffectConfig> listEffect = new List<EffectConfig>();

    public bool mIsActive { get; set; }
    protected GameObject lastObjCollision = null;
    protected int lastFrameCollision = 0;
    public BossController BossSourceUnit { get; set; }
    public PlayerUnit pUnitTarget { get; protected set; }
    public float LifeTimeDuration { get { return lifeTimeDuration; } set { lifeTimeDuration = value; } }
    public float LifeTime { get; set; }
    public long Damage { get; protected set; }

    protected PhotonView PV;
    protected virtual void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    protected void OnTriggerEnter2D(Collider2D col)
    {
        if (mIsActive)
        {
            var frameCount = Time.frameCount;
            var colObj = col.gameObject;
            if (lastObjCollision == colObj)
            {
                return;
            }

            lastFrameCollision = frameCount;
            lastObjCollision = colObj;
            OnCollisionOther(colObj);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == lastObjCollision)
        {
            lastObjCollision = null;
        }
    }

    protected virtual bool OnCollisionOther(GameObject other)
    {
        PlayerUnit pUnit = other.GetComponent<PlayerUnit>();
        return HandleTargetCollision(pUnit, other);
    }

    public virtual bool HandleTargetCollision(PlayerUnit unit, GameObject other)
    {
        if (unit != null)
        {
            if (unit.IsAlive() == false) return false;

            var re = OnCatchTarget(unit);
            if (re)
            {
                OnDeactiveDan();
            }

            return re;
        }
        else
        {
            if (OnHitObstacle(other))
            {
                OnDeactiveDan();
                return true;
            }
        }

        return false;
    }
    
    protected virtual void OnDeactiveDan()
    {
        mIsActive = false;
        gameObject.SetActive(false);
    }
    
    protected virtual bool OnCatchTarget(PlayerUnit unit)
    {
        if (unit.IsAlive() == false) return false;
        var takenDmg = unit.TakeDamage(
            new SatThuongInfo
            {
                dmg = Damage,
                enemySourceUnit = BossSourceUnit,
            });
       
        foreach (var eff in listEffect)
        {
            unit.GetStatusEff().ApplyEffect(new BattleEffect(eff));
        }

        return true;
    }
    
    protected virtual bool OnHitObstacle(GameObject obstacle)
    {
        if (obstacle.layer == LayersMan.LayerWall)
        { 
            return true;
        }
        return false; // mac dinh damage object xuyen obstacle
    }
    
      
    //gay dmg len player
    public virtual void ActiveDan(float speed, long dmg,
        Vector3 target, PlayerUnit playerTarget)
    {
        listEffect.Clear();
        Damage = dmg;
        mIsActive = true;
        gameObject.SetActive(true);
        this.LifeTime = lifeTimeDuration;
        lastObjCollision = null;
        pUnitTarget = playerTarget;
    }
    
    public virtual void DeactiveDan()
    {
        OnDeactiveDan();
    }
    
    protected virtual void Update()
    {

    }
}


public struct SatThuongInfo
{
    public long dmg { get; set; }
    public bool displayCrit { get; set; }
    public BossController enemySourceUnit { get; set; }
}