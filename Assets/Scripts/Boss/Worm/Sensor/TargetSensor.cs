using System.Collections;
using System.Collections.Generic;
using Goap.Classes;
using Goap.Classes.References;
using Goap.Interfaces;
using Goap.Sensors;
using UnityEngine;

public class TargetSensor : LocalTargetSensorBase
{
    public override void Created()
    {
    }

    public override void Update()
    {
    }

    public override ITarget Sense(IMonoAgent agent, IComponentReference references)
    {
        return new TransformTarget(agent.GetComponent<WormAgent>().Target);
    }
}