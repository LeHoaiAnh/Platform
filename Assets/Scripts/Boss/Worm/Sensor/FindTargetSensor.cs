using System.Collections;
using System.Collections.Generic;
using Goap.Classes;
using Goap.Classes.References;
using Goap.Interfaces;
using Goap.Sensors;
using UnityEngine;

public class FindTargetSensor : LocalWorldSensorBase
{
    public override void Created()
    {
    }

    public override void Update()
    {
    }

    public override SenseValue Sense(IMonoAgent agent, IComponentReference references)
    {
        var bossController = references.GetComponent<WormAgent>();

        if (bossController == null)
        {
            return false;
        }

        return bossController.Target != null;
    }
}
