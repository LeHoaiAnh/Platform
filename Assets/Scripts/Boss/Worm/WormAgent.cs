using System;
using System.Collections;
using System.Collections.Generic;
using Goap.Behaviours;
using Goap.Interfaces;
using Photon.Pun;
#if UNITY_EDITOR
using UnityEditor.Animations;
#endif
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

public enum WormState
{
    Idle
}
public class WormAgent : MonoBehaviourPun
{
    public Transform Target { get; set; }
    private AgentBehaviour agent;
    private IGoapRunner goapRunner;
    public BossController bossController { get; private set; }
    public int sungCls { get; set; }
    [SerializeField] private int totalTypeShoot;
    public SungCls parabolShoot;
    public bool DidAction { get; set; }

    
    //For animator
    public Animator animator { get; set; }
    private bool isInited;
    private int mStateAnim;
    public int mAttackAnim { get; set; }
    private WormState state;

    public PhotonView PV;
    private void Awake()
    {
        agent = GetComponent<AgentBehaviour>();
        goapRunner = GetComponentInChildren<GoapRunnerBehaviour>();
        bossController = GetComponent<BossController>();
        PV = GetComponent<PhotonView>();
    }

    private void OnEnable()
    {
        if (!isInited)
        {
            Init();
        }
    }

    private void Start()
    {
        agent.GoapSet = goapRunner.GetSet("Worm");
        agent.DoToGoal();
    }
    private void OnDisable()
    {
        isInited = false;
    }

    private void Init()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        if (animator != null)
        {
            mStateAnim = Animator.StringToHash("State");
            mAttackAnim = Animator.StringToHash("Attack");
        }

        state = WormState.Idle;
    }

    public void SwitchState(WormState state)
    {
        if (this.state == state)
        {
            return;
        }
        else
        {
            switch (state)
            {
            }

            this.state = state;
        }
    }

    public void ChooseTypeShoot()
    {
        sungCls = Random.Range(0, totalTypeShoot);
    }
    
    void ShootParabol()
    {
        var shooter = parabolShoot;
        shooter.FireAt(
            Target != null ? Target.position : Vector3.zero,
            Target != null ? Target.GetComponent<PlayerUnit>() : null);
    }

    public void Shoot()
    {
        if (PhotonNetwork.InRoom)
        {
            if(PV != null && PV.IsMine)
            {
                //PV.RPC("RPC_Shoot", RpcTarget.AllBufferedViaServer);
                RPC_Shoot();
            }
        }
        else
        {
            RPC_Shoot();
        }
    }

    [PunRPC]
    public void RPC_Shoot()
    {
        switch (sungCls)
        {
            case 0:
                ShootParabol();
                break;
        }
    }
    public bool CanShoot()
    {
        switch (sungCls)
        {
            case 0:
                return !parabolShoot.IsCoolDown;
        }

        return false;
    }
}
