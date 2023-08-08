using System.Collections;
using System.Collections.Generic;
using Goap.Behaviours;
using Goap.Enums;
using Goap.Interfaces;
using UnityEngine;

public class Idle : ActionBase<Idle.Data>
{
    public class Data : IActionData
    {
        public ITarget Target { get; set; }
    }

    public override void Created(){}


    public override void Start(IMonoAgent agent, Data data)
    {
        
    }

    public override ActionRunState Perform(IMonoAgent agent, Data data, float intervalTime)
    {
        return ActionRunState.Stop;
    }

    public override void End(IMonoAgent agent, Data data)
    {
        
    }
}
