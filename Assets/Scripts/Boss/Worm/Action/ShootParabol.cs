using System.Collections;
using System.Collections.Generic;
using Goap.Behaviours;
using Goap.Enums;
using Goap.Interfaces;
using Photon.Pun;
using UnityEngine;

public class ShootParabol :  ActionBase<ShootParabol.Data>, IPunObservable
{
    private WormAgent bossAgent;
    private float currentTime;

    private bool Photnet_AnimSyncAttack = false;
    public class Data : IActionData
    {
        public ITarget Target { get; set; }
        public float totalTime;
    }
    
    public override void Created(){}


    public override void Start(IMonoAgent agent, Data data)
    {
        bossAgent = agent.GetComponent<WormAgent>();
        data.totalTime = 1f;
        currentTime = 0;
    }

    public override ActionRunState Perform(IMonoAgent agent, Data data, float intervalTime)
    {
        if (currentTime == 0)
        {
            Photnet_AnimSyncAttack = true;
            bossAgent.animator.SetTrigger(bossAgent.mAttackAnim);
        }
        if (currentTime >= data.totalTime)
        {
            bossAgent.DidAction = true;
            bossAgent.sungCls = -1;
            return ActionRunState.Stop;
        }
        else
        {
            currentTime += intervalTime;
            return ActionRunState.Continue;
        }
    }

    public override void End(IMonoAgent agent, Data data)
    {
        
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(Photnet_AnimSyncAttack);
            Photnet_AnimSyncAttack = false;
        }
        else if (stream.IsReading)
        {
            if (Photnet_AnimSyncAttack)
            {
                bossAgent.animator.SetTrigger(bossAgent.mAttackAnim);
                Photnet_AnimSyncAttack = false;
            }
        }
    }

    /*void Shoot()
    {
        var shooter = bossAgent.parabolShoot;
        shooter.FireAt(
            bossAgent.Target != null ? bossAgent.Target.position : Vector3.zero,
            bossAgent.Target != null ? bossAgent.Target.GetComponent<PlayerUnit>() : null);
    }*/
}
