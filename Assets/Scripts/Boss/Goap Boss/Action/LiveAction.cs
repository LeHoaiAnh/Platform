using System.Collections;
using System.Collections.Generic;
using Goap.Behaviours;
using Goap.Enums;
using Goap.Interfaces;
using UnityEngine;

public class LiveAction : ActionBase<LiveAction.Data>
{
    private BossController bossController;
    private BossAgent bossAgent;
    private float curTime;
    public class Data : IActionData
    {
        public ITarget Target { get; set; }
        public float timer;
    }
    
    public override void Created()
    {
    }

    public override void Start(IMonoAgent agent, Data data)
    {
        bossController = agent.GetComponent<BossController>();
        bossAgent = agent.GetComponent<BossAgent>();
        data.timer = 5f;
        curTime = 0;
    }

    public override ActionRunState Perform(IMonoAgent agent, Data data, float intervalTime)
    {
        if (curTime == 0)
        {
            bossController.animator.SetBool(bossController.mAnimSkill, true);
            bossAgent.ActiveSkill();
        }

        curTime += intervalTime;

        if (curTime < data.timer)
        {
            return ActionRunState.Continue;
        }
        else
        {
            return ActionRunState.Stop;
        }
    }

    public override void End(IMonoAgent agent, Data data)
    {
        bossController.animator.SetBool(bossController.mAnimSkill, false);
        bossController.currentStats.timeReloadSkill = 0;
        bossAgent.CancleSkill();
    }

    public override float GetInRange(IMonoAgent agent, IActionData data)
    {
        return 0.01f;
    }
}
