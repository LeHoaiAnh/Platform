using System.Collections;
using System.Collections.Generic;
using Goap.Behaviours;
using Goap.Enums;
using Goap.Interfaces;
using UnityEngine;

public class RunTowardTargetAction : ActionBase<RunTowardTargetAction.Data>
{
    public class Data : IActionData
    {
        public ITarget Target { get; set; }
        public float Timer { get; set; }
    }
    
    public override void Created(){}
    public override void Start(IMonoAgent agent, Data data)
    {
        data.Timer = Random.Range(0.3f, 1f);
        agent.GetComponent<BossController>().currentStats.speed *= 3f;
    }

    public override ActionRunState Perform(IMonoAgent agent, Data data, float intervalTime)
    {
        data.Timer -= intervalTime;
        if (data.Timer > 0)
            return ActionRunState.Continue;
        
        return ActionRunState.Stop;
    }

    public override void End(IMonoAgent agent, Data data)
    {
        agent.GetComponent<BossController>().currentStats.speed /= 3f;
    }
}
