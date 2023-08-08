using System.Collections;
using System.Collections.Generic;
using Goap.Enums;
using Goap.Behaviours;
using Goap.Interfaces;
using UnityEngine;

public class WanderAction : ActionBase<WanderAction.Data>
{
    public class Data : IActionData
    {
        public ITarget Target { get; set; }
    }

    public override void Created()
    {
    }

    public override void Start(IMonoAgent agent, Data data)
    {
    }

    public override ActionRunState Perform(IMonoAgent agent, Data data, float intervalTime)
    {
        agent.GetComponent<BossAgent>().IsWandering = true;
        return ActionRunState.Stop;
    }

    public override void End(IMonoAgent agent, Data data)
    {
        
    }

    public override float GetInRange(IMonoAgent agent, IActionData data)
    {
        return 0.0001f;
    }
}
