using System.Collections;
using System.Collections.Generic;
using Goap.Behaviours;
using Goap.Classes.References;
using Goap.Enums;
using Goap.Interfaces;
using UnityEngine;

public class AttackPlayerAction : ActionBase<AttackPlayerAction.Data>
{
    private BossAgent bossAgent;
    private BossController bossController;
    private float curTime;
    public class Data : IActionData
    {
        public ITarget Target { get; set; }
        public float timer;
    }
    
    public override void Created(){}
    public override void Start(IMonoAgent agent, Data data)
    {
        bossAgent = agent.GetComponent<BossAgent>();
        bossController = agent.GetComponent<BossController>();
        bossController.SetSpeed(bossController.initStats.speed * 3);
        data.timer = 0.5f;
        curTime = 0;
    }

    public override float GetInRange(IMonoAgent agent, IActionData data)
    {
        return bossController.currentStats.rangeATK;
    }

    public override ActionRunState Perform(IMonoAgent agent, Data data, float intervalTime)
    {
        if (curTime == 0)
        {
            bossAgent.RotataTransform(bossController.target.transform.position);
            bossController.SetSpeed(bossController.initStats.speed);
            bossController.animator.SetTrigger(bossController.mAnimAttack);
        }

        curTime += intervalTime;

        if (curTime < data.timer)
        {
            return ActionRunState.Continue;
        }
        else
        {
            bossController.target.GetEnergy(-bossController.currentStats.attackDmg);
            return ActionRunState.Stop;

        }
    }
    
    public override void End(IMonoAgent agent, Data data)
    {
        bossController.SetSpeed(bossController.initStats.speed);
        bossController.currentStats.timeReload = 0;
    }
    
}
