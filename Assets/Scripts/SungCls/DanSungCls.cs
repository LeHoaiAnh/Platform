using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanSungCls : SatThuongDT
{
    public float Range = 30;
    public float Speed { get; protected set; }
    public float TravelDistance { get; set; } = 0;
    public float TravelMaxDistance { get; set; }

    protected PlayerUnit playerTarget;
    public GameObject DestroyEff = null;
    public float LifeTime { get; set; }
    protected Vector3 dir;

    public override void ActiveDan(float speed, long dmg, Vector3 target, PlayerUnit unitTarget)
    {
        base.ActiveDan(speed, dmg, target, unitTarget);
        Speed = speed;
        playerTarget = unitTarget;
        TravelDistance = 0;
        TravelMaxDistance = Range;
        LifeTime = lifeTimeDuration;
        dir = transform.position.x < target.x ? Vector3.right : Vector3.left;
    }
    
    protected override void OnDeactiveDan()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.Destroy(gameObject);
        }
        else
        {
            mIsActive = false;
            gameObject.SetActive(false);

            if (DestroyEff != null)
            {
                GameObject des_eff = ObjectPoolManager.SpawnAutoUnSpawn(DestroyEff, 3f);
                des_eff.transform.position = transform.position;
            }
            ObjectPoolManager.Unspawn(gameObject);
        }
    }
    
    public override void DeactiveDan()
    {
        Debug.Log("DeactiveDan" + this.name);
        OnDeactiveDan();
    }

    protected override void Update()
    {
        if (PhotonNetwork.InRoom)
        {
            if (PV != null && PV.IsMine == false) return;
        }

        if (mIsActive)
        {
            if (PlayerManagement.Instance == null)
            {
                DeactiveDan();
                return;
            }

            float distance = Speed * Time.deltaTime;
            transform.position += distance * dir;
            TravelDistance += distance;
        }

        if (LifeTime > 0 && TravelDistance < TravelMaxDistance)
        {
            LifeTime -= Time.deltaTime;
        }
        else
        {
            DeactiveDan();
        }
    }

}
