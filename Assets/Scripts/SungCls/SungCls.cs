using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SungCls : MonoBehaviour
{
    #region Configuration

    [SerializeField]
    SatThuongDT[] danPrefabArray;
    [SerializeField]
    Transform[] barrels;
    [SerializeField]
    int DMG = 10;
    [SerializeField]
    int clipSize = 10; // so dan trong bang dan
    [SerializeField]
    float reloadTime = 0f;
    [SerializeField]
    int TypeDan = 0;
    [SerializeField]
    float ProjSpd = 10;
    #endregion
    protected BossController eUnit;
    float mCoolDown = -10f;
    bool getFired = false;
    protected int clip = 0;
    protected float reloadingTime = 0;


    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        eUnit = GetComponentInParent<BossController>();
        mCoolDown = -10f;
        InitFireSoundSource();

        getFired = false;

        clip = clipSize;
        reloadingTime = 0;
    }
    
    AudioSource FireSource = null;

    void InitFireSoundSource()
    {
        if (FireSource == null)
            FireSource = gameObject.AddComponent<AudioSource>();
        FireSource.spatialBlend = 1f;
        FireSource.rolloffMode = AudioRolloffMode.Linear;
        FireSource.minDistance = 4f;
        FireSource.maxDistance = 40f;
        FireSource.volume = 0.7f;
    }
    
    float mTimeSinceLastFire = 0;

    void Start()
    {
        mTimeSinceLastFire = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (mCoolDown > -10f)
        {
            mCoolDown -= Time.deltaTime;
        }

        if (reloadingTime > 0)
        {
            reloadingTime -= Time.deltaTime;
        }

        mTimeSinceLastFire += Time.deltaTime;
    }
    
    void Reload()
    {
        clip = clipSize;
        reloadingTime = reloadTime;
    }
    
    public bool IsReloading { get { return reloadingTime > 0; } }
    public bool IsCoolDown { get { return mCoolDown > 0; } }


    public bool FireAt(Vector3 targetPos, PlayerUnit playerUnit)
    {
        if (IsReloading) return false;
        if (reloadTime > 0 && clip <= 0) return false;
        if (mCoolDown <= 0)
        {
            var trans = transform;
            if (barrels != null && barrels.Length > 0)
            {
                for (int i = 0; i < barrels.Length; ++i)
                {
                    trans = barrels[i];
                    
                    if (DelayStartProjectile > 0)
                    {
                        var barrelTrans = trans;
                        HoaiAnh.Utils.DoAction(this, () => FireByTrans(barrelTrans, targetPos, playerUnit), DelayStartProjectile);
                    }
                    else
                    {
                        FireByTrans(trans, targetPos, playerUnit);
                    }
                }
            }
            
            var atkTime = 1f / AtkSpd;
            //mCoolDown += atkTime;
            mCoolDown = atkTime;
            //Debug.Log("mCoolDown= " + mCoolDown);

            if (reloadTime > 0)
            {
                clip--;
                if (clip <= 0)
                {
                    if (DelayStartProjectile > 0)
                    {
                        HoaiAnh.Utils.DoAction(this, Reload, DelayStartProjectile);
                    }
                    else
                    {
                        Reload();
                    }
                }
            }

            if (getFired == false)
            {
                getFired = true;
            }

            return true;
        }

        return false;
    }

    void FireByTrans(Transform trans, Vector3 targetPos, PlayerUnit unit)
    {
        if (unit != null)
        {
            var proj = FireShot(trans, targetPos, Vector3.zero, unit);
        }
    }

    SatThuongDT FireShot(Transform trans, Vector3 targetPos, Vector3 displacement, PlayerUnit target)
    {
        if (ProjSpd <= 0) return null;

        bool isDanNotNull = TypeDan < danPrefabArray.Length && danPrefabArray[TypeDan] != null;
        if (isDanNotNull == false) Debug.LogFormat("[{0}]Dan prefab is null", eUnit.gameObject.name);

        GameObject projObj = null;

        if (isDanNotNull)
        {
            if (PhotonNetwork.InRoom)
            {
                projObj = PhotonNetwork.Instantiate("Prefabs/Bullet/" + danPrefabArray[TypeDan].gameObject.name, trans.position + displacement, trans.rotation);
            }
            else
            {
                projObj = ObjectPoolManager.Spawn(danPrefabArray[TypeDan].gameObject);
            }
        }
        else
        {
            return null;
        }
        var proj = projObj.GetComponent<SatThuongDT>();
        
        proj.transform.rotation = trans.rotation;
        proj.transform.position = trans.position + displacement;
        proj.transform.position = new Vector3(proj.transform.position.x, proj.transform.position.y, 0);

        ActiveDmgObj(proj, targetPos, target);
        proj.gameObject.SetActive(true);
        
        return proj;
    }

      void ActiveDmgObj(SatThuongDT dmgObj, Vector3 targetPos, PlayerUnit target)
      {
          var dmg = DMG;

          mTimeSinceLastFire = 0;
          float projSpd = ProjSpd;
        

          dmgObj.BossSourceUnit = eUnit;

          if(target != null) dmgObj.ActiveDan(projSpd, dmg, targetPos, target);

          CheckDamageEff(dmgObj);
      }  
      
      public List<EffectConfig> effects = new List<EffectConfig>();

      void CheckDamageEff(SatThuongDT proj)
      {
          proj.listEffect.Clear();

          if (effects != null)
          {
              if (effects.Count > 0)
              {
                  foreach (EffectConfig e in effects)
                  {
                      proj.listEffect.Add(e.Clone());
                  }
              }
          }
      }

    [SerializeField] private float AtkSpd = 1;
    [SerializeField] private float AttackAnimTime = 1f;

    public float AtkSpdAnimScale
    {
        get
        {
            var atkTime = 1f / AtkSpd;
            if (atkTime < AttackAnimTime) // only speed up anim
            {
                return AttackAnimTime / atkTime;
            }

            return 1;
        }
    }
    [SerializeField] float DelayActiveDamage = 0;
    public float DelayStartProjectile
    {
        get
        {
            return DelayActiveDamage > 0 ?
                Mathf.Max(0.1f, AtkSpdAnimScale > 1 ? (float)(DelayActiveDamage / AtkSpdAnimScale) : (float)DelayActiveDamage) :
                0;
        }
    }
}
