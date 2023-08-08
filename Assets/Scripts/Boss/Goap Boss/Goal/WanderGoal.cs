using System.Collections;
using System.Collections.Generic;
using Goap.Behaviours;
using Goap.Interfaces;
using UnityEngine;

public class WanderGoal : GoalBase
{
    public override int GetIntensity()
    {
        return 1;
    }
}
