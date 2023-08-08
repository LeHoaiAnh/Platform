using System.Collections;
using System.Collections.Generic;
using Goap.Behaviours;
using Goap.Configs.Interfaces;
using UnityEngine;
using Goap.Classes.Builders;
using Goap.Enums;
using Goap.Resolver;

public class WormConfigFactory : GoapSetFactoryBase
{
    public override IGoapSetConfig Create()
    {
        var builder = new GoapSetBuilder("Worm");
        
        //Goals
        AddLiveGoal(builder);
        
        //Actions
        AddFindTargetAction(builder);
        AddIdleAction(builder);
        AddChooseTypeShootAction(builder);
        AddShootParabolAction(builder);
        
        // TargetSensors
        AddTargetSensor(builder);

        //World Sensor
        AddFindTargetSensor(builder);
        AddCanShootSensor(builder);
        AddDisChooseTypeShootSensor(builder);
        AddLiveSensor(builder);
        
        return builder.Build();
    }
    
    public void AddLiveGoal(GoapSetBuilder builder)
    {
        builder.AddGoal<LiveGoal>()
            .AddCondition(WorldKeys.Live, Comparision.GreaterThanOrEqual, 1);
    }

    public void AddFindTargetAction(GoapSetBuilder builder)
    {
        builder.AddAction<FindTargetAction>().SetTarget(Targets.Target)
            .SetMoveMode(ActionMoveMode.PerformNotMoving)
            .AddEffect(WorldKeys.DidFindTarget, true);
    }
    
    public void AddChooseTypeShootAction(GoapSetBuilder builder)
    {
        builder.AddAction<ChooseTypeShoot>().SetTarget(Targets.Target)
            .AddCondition(WorldKeys.DidFindTarget, Comparision.GreaterThanOrEqual, 1)
            .SetMoveMode(ActionMoveMode.PerformNotMoving)
            .AddEffect(WorldKeys.DidChooseTypeShoot, true)
            .SetBaseCost(20);
    }
    
    private void AddTargetSensor(GoapSetBuilder builder)
    {
        builder.AddTargetSensor<TargetSensor>()
            .SetTarget(Targets.Target);
    }
    
    public void AddFindTargetSensor(GoapSetBuilder builder)
    {
        builder.AddWorldSensor<FindTargetSensor>()
            .SetKey(WorldKeys.DidFindTarget);
    }
    public void AddDisChooseTypeShootSensor(GoapSetBuilder builder)
    {
        builder.AddWorldSensor<DisChooseTypeShootSensor>()
            .SetKey(WorldKeys.DidChooseTypeShoot);
    }

    public void AddCanShootSensor(GoapSetBuilder builder)
    {
        builder.AddWorldSensor<CanShootSensor>()
            .SetKey(WorldKeys.CanShoot);
    }
    
    public void AddIdleAction(GoapSetBuilder builder)
    {
        builder.AddAction<Idle>()
            .SetMoveMode(ActionMoveMode.PerformNotMoving)
            .AddEffect(WorldKeys.Live, true)
            .SetBaseCost(150);
    }
    
    public void AddShootParabolAction(GoapSetBuilder builder)
    {
        builder.AddAction<ShootParabol>().AddCondition(WorldKeys.DidChooseTypeShoot, Comparision.GreaterThanOrEqual, 1)
            .SetMoveMode(ActionMoveMode.PerformNotMoving)
            .AddCondition(WorldKeys.CanShoot, Comparision.GreaterThanOrEqual, 1)
            .AddEffect(WorldKeys.Live, true)
            .SetBaseCost(30);
    }
    
    public void AddLiveSensor(GoapSetBuilder builder)
    {
        builder.AddWorldSensor<WormLiveSensor>()
            .SetKey(WorldKeys.Live);
    }
}
