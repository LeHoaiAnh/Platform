using System.Collections;
using System.Collections.Generic;
using Goap.Classes;
using Goap.Classes.References;
using Goap.Interfaces;
using Goap.Sensors;
using UnityEngine;

public class AttackPlayerSensor : LocalTargetSensorBase
{
    public override void Created()
    {
    }

    public override void Update()
    {
        
    }
    
    public override ITarget Sense(IMonoAgent agent, IComponentReference references)
    {
        if (PlayerManagement.Instance == null)
        {
            Debug.LogError("Check logic");
            return new TransformTarget(agent.transform);
        }

        return new TransformTarget(PlayerManagement.Instance.PlayerUnit.transform);
    }
}

