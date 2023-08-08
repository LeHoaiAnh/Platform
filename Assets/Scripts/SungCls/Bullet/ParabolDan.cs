using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ParabolDan : SatThuongDT
{
    public GameObject DestroyEff = null;

    public float Speed { get; protected set; }

    private Transform unitTargetTransform;
    private Vector2 startPos;
    private Vector2 endPos;
    

    public override void ActiveDan(float speed, long dmg, Vector3 target, PlayerUnit unitTarget)
    {
        base.ActiveDan(speed, dmg, target, unitTarget);
        this.Speed = speed;
        startPos = transform.position;
        endPos = target;
    }
    
    public override void DeactiveDan()
    {
        OnDeactiveDan();
    }
    
    protected override void OnDeactiveDan()
    {
        mIsActive = false;
        gameObject.SetActive(false);

        if (DestroyEff != null)
        {
            GameObject des_eff = ObjectPoolManager.SpawnAutoUnSpawn(DestroyEff, 10f);
            des_eff.transform.position = this.transform.position;
        }
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.Destroy(gameObject);
        }
        else
        {
            ObjectPoolManager.Unspawn(gameObject);
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (mIsActive)
        {
            if (QuanLyManChoi.Instance == null || QuanLyManChoi.Instance.gameState != GAMESTATE.INGAME)
            {
                DeactiveDan();
                return;
            }

            if (transform.position.z > 0 || Vector3.Distance(transform.position, endPos) <= 0.5f)
            {
                DeactiveDan();
            }
            else
            {
                Vector2 currentPos = MathParabola.Parabola(startPos, endPos, 3, LifeTimeDuration - LifeTime);
                transform.position = new Vector3( currentPos.x, currentPos.y, 0);
                LifeTime -= Time.deltaTime;
            }

        }
    }

    public void OverrideSpd(float spd)
    {
        this.Speed = spd;
    }
    public void OverrideDmg(long d)
    {
        this.Damage = d;
    }
}

public class MathParabola
{

    public static Vector3 Parabola(Vector3 start, Vector3 end, float height, float t)
    {
        Func<float, float> f = x => -4 * height * x * x + 4 * height * x;

        var mid = Vector3.Lerp(start, end, t);

        return new Vector3(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t), mid.z);
    }

    public static Vector2 Parabola(Vector2 start, Vector2 end, float height, float t)
    {
        Func<float, float> f = x => -4 * height * x * x + 4 * height * x;

        var mid = Vector2.Lerp(start, end, t);

        return new Vector2(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t));
    }

}