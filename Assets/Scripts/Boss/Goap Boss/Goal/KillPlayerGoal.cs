using System.Collections;
using System.Collections.Generic;
using Goap.Behaviours;
using UnityEngine;

public class KillPlayerGoal : GoalBase
{
    public override int GetIntensity()
    {
        return 5;
    }
}
