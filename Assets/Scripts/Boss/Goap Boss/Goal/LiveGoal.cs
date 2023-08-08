using System.Collections;
using System.Collections.Generic;
using Goap.Behaviours;
using UnityEngine;

public class LiveGoal : GoalBase
{
    public override int GetIntensity()
    {
        return 10;
    }
}
