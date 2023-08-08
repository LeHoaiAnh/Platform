using System.Collections;
using System.Collections.Generic;
using Goap.Classes;
using Goap.Classes.References;
using Goap.Interfaces;
using Goap.Sensors;
using UnityEngine;

public class WanderTargetSensor : LocalTargetSensorBase
{
    public override void Created()
    {
    }

    public override void Update()
    {
    }
    
    public override ITarget Sense(IMonoAgent agent, IComponentReference references)
    {
        var pos = references.GetComponent<BossAgent>().GetTargetWander();

        return new PositionTarget(pos);
    }
    
    
}
