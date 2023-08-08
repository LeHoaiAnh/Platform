using System;
using System.Collections;
using System.Collections.Generic;
using HoaiAnh;
using HoaiAnh.Util;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

[Serializable]
public class BossStats
{
    public long HP;
    public float speed;
    public long attackDmg;
    public float timeReload;
    public float rangeATK;
    public float seeRange;
    public float timeReloadSkill;

    public float param1;
    public BossStats()
    {
        
    }

    public BossStats Clone()
    {
        BossStats tmp = new BossStats();
        tmp.HP = HP;
        tmp.speed = speed;
        tmp.attackDmg = attackDmg;
        tmp.timeReload = timeReload;
        tmp.rangeATK = rangeATK;
        tmp.seeRange = seeRange;
        tmp.param1 = param1;
        tmp.timeReloadSkill = timeReloadSkill;
        return tmp;
    }
}
public class BossController : MonoBehaviourPun, IPunObservable
{
    public BossStats initStats;
    public BossStats currentStats;
    public Vector3 OffsetHUD;
    public bool hasHPBar = false;
    public PlayerUnit target;
    private HPBar mCurHPBar;
    public Animator animator { get; set; }

    public int mAnimAttack { get; set; }
    public int mAnimSkill { get; set; }
    public int mAnimDie { get; set; }
    public int mAnimHit { get; set; }

    #region  Heal hp

    protected float healHpSpeed;
    protected float currentTimeHealHp;
    
    #endregion
    
    public delegate void ActionDelegate();
    public event ActionDelegate OnUnitDieAction;
    public EffectVisual[] EffectVisuals;
    ManagementEffectBoss mStatusEffect;
    public SungCls shooter { get; set; }

    public PhotonView PV;
    protected bool Photnet_AnimSyncDie = false;
    protected bool Photnet_AnimSyncHit = false;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    public ManagementEffectBoss GetStatusEff()
    {
        if (mStatusEffect == null)
        {
            mStatusEffect = gameObject.AddMissingComponent<ManagementEffectBoss>();
        }
        return mStatusEffect;
    }
    public void OnUnitDie()
    {
        animator.SetTrigger(mAnimDie);
        Photnet_AnimSyncDie = true; 
        OnUnitDieAction?.Invoke();
    }
    public bool CanAttack()
    {
        return currentStats.timeReload >= initStats.timeReload;
    }
    public bool CanUsingSkill()
    {
        return currentStats.timeReloadSkill >= initStats.timeReloadSkill;
    }
    
    private void Start()
    {
        Init();
    }
    
    void Init()
    {
        currentStats = initStats.Clone();
        if (ScreenBattle.Instance != null && hasHPBar && mCurHPBar == null)
        {
            mCurHPBar = ScreenBattle.Instance.CreateHPBar(this);
        }

        animator = GetComponent<Animator>();
        if (animator != null)
        {
            mAnimAttack = Animator.StringToHash("Attack");
            mAnimSkill = Animator.StringToHash("Skill");
            mAnimDie = Animator.StringToHash("Death");
            mAnimHit = Animator.StringToHash("Hit");
        }

        shooter = GetComponent<SungCls>();
        SetTarget();
    }

    public void SetTarget()
    {
        if (target != null)
        {
            if (Vector3.Distance(transform.position, target.transform.position) < currentStats.seeRange)
            {
                return;
            }
        }
        Collider2D[] colliders = ArrayPool<Collider2D>.Claim(30);
        int numHit = Physics2D.OverlapCircleNonAlloc(transform.position, currentStats.seeRange, colliders, LayerMask.GetMask("Player"));

        //foreach (var VARIABLE in colliders)
        PlayerUnit bot = null;
        for (int i = 0; i < numHit; ++i)
        {
            var newBot = colliders[i].GetComponent<PlayerUnit>();
            if (newBot != null)
            {
                if (bot == null)
                {
                    bot = newBot;
                }
                else
                {
                    if (Vector3.Distance(transform.position, bot.transform.position) > Vector3.Distance(transform.position, newBot.transform.position))
                    {
                        bot = newBot;
                    }
                }
            }
        }
        if (bot != null)
        {
            target = bot;
        }
        ArrayPool<Collider2D>.Release(ref colliders);
    }

    private void Update()
    {
        if (PhotonNetwork.InRoom)
        {
            if (PhotonNetwork.IsMasterClient == false) return;
        }
        if (IsAlive())
        {
            currentStats.timeReload += Time.deltaTime;
            currentStats.timeReloadSkill += Time.deltaTime;

            if (healHpSpeed > 0)
            {
                currentTimeHealHp += Time.deltaTime;
                if (currentTimeHealHp >= 1f)
                {
                    UpdateHP((long)(healHpSpeed * initStats.HP));
                    currentTimeHealHp = 0;
                }
            }
            
            ProcessHud();

            SetTarget();
        }
    }
    

    public bool IsAlive()
    {
      return currentStats.HP > 0;
    }

    public void UpdateHP(long change)
    {
        if (PhotonNetwork.InRoom)
        {
            if(PhotonNetwork.IsMasterClient && PV != null)
            {
                PV.RPC("RPC_UpdateHP", RpcTarget.AllBufferedViaServer, change);
            }
        }
        else
        {
            UpDateCurrentHP(change);
        }
    }

    private void UpDateCurrentHP(long change)
    {
        currentStats.HP += change;
        currentStats.HP = Math.Min(currentStats.HP, initStats.HP);
        currentStats.HP = Math.Max(currentStats.HP, 0);
        if (mCurHPBar)
        {
            mCurHPBar.UpdateHP();
        }

        if (change < 0 && IsAlive())
        {
            animator.SetTrigger(mAnimHit);
            Photnet_AnimSyncHit = true;
        }

        if (!IsAlive())
        {
            OnUnitDie();
            if (PhotonNetwork.InRoom)
            {
                PhotonNetwork.Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    [PunRPC]
    void RPC_UpdateHP(long change)
    {
        UpDateCurrentHP(change);
    }

    public HPBar GetCurHPBar()
      {
          return mCurHPBar;
      }

      public void SetSpeed(float speed)
      {
          currentStats.speed = speed;
          animator.speed = currentStats.speed / initStats.speed;
      }

      public void HealHpPerTime(float speed)
      {
          healHpSpeed = speed;
          currentTimeHealHp = 0;
          UpdateHP((long)(healHpSpeed * initStats.HP));
      } 
      
      private Queue<ScreenBattle.DisplayHUDEnemyCommand> queueHUDs = new Queue<ScreenBattle.DisplayHUDEnemyCommand>();

      float hudCoolDown = 0;
      private void ProcessHud()
      {
          if (hudCoolDown > 0)
          {
              hudCoolDown -= Time.unscaledDeltaTime;
          }
          else
          {
              if (queueHUDs.Count > 0)
              {
                  var command = queueHUDs.Dequeue();
                  var originText = command.Text;
                  ScreenBattle.Instance.DisplayHUD(command);
                  hudCoolDown = 0.065f;
              }
          }
      }
    
      public void QueueHudDisplay(ScreenBattle.DisplayHUDEnemyCommand hudCommand)
      {
          queueHUDs.Enqueue(hudCommand);
      }

      public int GetCurrentHUDQueue()
      {
          return queueHUDs.Count;
      }

      public bool CheckTargetInRange()
      {
          if (target == null)
          {
              SetDefaultTarget();
              return false;
          }
          return Vector3.Distance(transform.position, target.transform.position) <= currentStats.seeRange;
      }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(currentStats.HP);
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);

            stream.SendNext(Photnet_AnimSyncDie);
            Photnet_AnimSyncDie = false;

            stream.SendNext(Photnet_AnimSyncHit);
            Photnet_AnimSyncHit = false;
        }
        else if (stream.IsReading)
        {
            currentStats.HP = (long)stream.ReceiveNext();
            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();

            Photnet_AnimSyncDie = (bool)stream.ReceiveNext();
            if (Photnet_AnimSyncDie)
            {
                animator.SetTrigger(mAnimDie);
                Photnet_AnimSyncDie = false;
            }

            Photnet_AnimSyncHit = (bool)stream.ReceiveNext();
            if (Photnet_AnimSyncHit)
            {
                animator.SetTrigger(mAnimHit);
                Photnet_AnimSyncHit = false;
            }
        }
    }

    public void SetDefaultTarget()
    {
        SetTarget();
    }
}
