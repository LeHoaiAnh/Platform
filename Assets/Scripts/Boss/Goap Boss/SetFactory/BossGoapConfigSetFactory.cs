using System.Collections;
using System.Collections.Generic;
using Goap.Behaviours;
using Goap.Classes;
using Goap.Classes.Builders;
using Goap.Configs;
using Goap.Configs.Interfaces;
using Goap.Resolver;
using UnityEditor;
using UnityEngine;

public static class WorldKeys
{
    public static string IsWandering => "IsWandering";
    public static string AttackDone => "AttackDone";
    public static string HPPlayer => "HPPlayer";
    public static string CanAttack => "CanAttack";
    public static string SeeTarget => "SeeTarget";
    public static string LowHP => "LowHP";
    public static string CanUsingSkill => "CanUsingSkill";
    public static string UsedSkill => "UsedSkill";
    
    //For Worm
    public static string Live => "Live";
    public static string DidFindTarget => "DidFindTarget";
    public static string CanShoot => "CanShoot";
    public static string DidChooseTypeShoot => "DidChooseTypeShoot";
}

public static class Targets
{
    public const string WanderTarget = "WanderTarget";
    public const string AttackTarget = "AttackTarget";
    public const string SkillTarget = "SkillTarget";
    
    //For Worm
    public const string Target = "Target";
}

public class BossGoapConfigSetFactory : GoapSetFactoryBase
{
    public override IGoapSetConfig Create()
    {
        var builder = new GoapSetBuilder("Boss");
        
        //Goals
        AddWanderGoal(builder);
        AddAtackPlayerGoal(builder);
        AddLiveGoal(builder);
        
        //Actions
        AddWanderAction(builder);
        AddAttackAction(builder);
        AddLiveAction(builder);
        
        // TargetSensors
        AddWanderTargetSensor(builder);
        AddTransformTargetSensor(builder);
        AddTargetSkillSensor(builder);

        //World Sensor
        AddCanSeeSensor(builder);
        AddCanAttackSensor(builder);
        AddIsWanderingSensor(builder);
        AddAttackDoneSensor(builder);
        AddLowHpSensor(builder);
        AddUsedSkill(builder);
        AddCanUseSkill(builder);
        
        return builder.Build();
    }

    private void AddWanderTargetSensor(GoapSetBuilder builder)
    {
        builder.AddTargetSensor<WanderTargetSensor>()
            .SetTarget(Targets.WanderTarget);
    }

    private void AddTransformTargetSensor(GoapSetBuilder builder)
    {
        builder.AddTargetSensor<TargetTransformSensor>()
            .SetTarget(Targets.AttackTarget);
    }
    
    public void AddWanderGoal(GoapSetBuilder builder)
    {
        builder.AddGoal<WanderGoal>()
            .AddCondition(WorldKeys.IsWandering, Comparision.GreaterThanOrEqual, 1);
    }

    public void AddWanderAction(GoapSetBuilder builder)
    {
        builder.AddAction<WanderAction>()
            .SetTarget(Targets.WanderTarget)
            .AddEffect(WorldKeys.IsWandering, true);
    }

    public void AddAtackPlayerGoal(GoapSetBuilder builder)
    {
        builder.AddGoal<KillPlayerGoal>().AddCondition(WorldKeys.AttackDone, Comparision.GreaterThanOrEqual, 1);
    }

    public void AddAttackAction(GoapSetBuilder builder)
    {
        builder.AddAction<AttackPlayerAction>().SetTarget(Targets.AttackTarget)
            .AddEffect(WorldKeys.CanAttack, false)
            .AddEffect(WorldKeys.AttackDone, true)
            .AddEffect(WorldKeys.HPPlayer, false)
            .AddCondition(WorldKeys.CanAttack, Comparision.GreaterThanOrEqual, 1)
            .AddCondition(WorldKeys.SeeTarget, Comparision.GreaterThanOrEqual, 1);
    }

    public void AddCanSeeSensor(GoapSetBuilder builder)
    {
        builder.AddWorldSensor<CanSeeSensor>()
            .SetKey(WorldKeys.SeeTarget);
    }
    
    public void AddCanAttackSensor(GoapSetBuilder builder)
    {
        builder.AddWorldSensor<CanAttackSensor>()
            .SetKey(WorldKeys.CanAttack);
    }
    
    public void AddIsWanderingSensor(GoapSetBuilder builder)
    {
        builder.AddWorldSensor<IsWanderingSensor>()
            .SetKey(WorldKeys.IsWandering);
    }
    
    public void AddHPPlayerSensor(GoapSetBuilder builder)
    {
        builder.AddWorldSensor<HPPlayerSensor>()
            .SetKey(WorldKeys.HPPlayer);
    }
    
    public void AddAttackDoneSensor(GoapSetBuilder builder)
    {
        builder.AddWorldSensor<AttackDoneSensor>()
            .SetKey(WorldKeys.AttackDone);
    }

    #region Live
    public void AddLiveGoal(GoapSetBuilder builder)
    {
        builder.AddGoal<LiveGoal>()
            .AddCondition(WorldKeys.UsedSkill, Comparision.GreaterThanOrEqual, 1);
    }
    
    public void AddLiveAction(GoapSetBuilder builder)
    {
        builder.AddAction<LiveAction>()
            .SetTarget(Targets.SkillTarget)
            .AddCondition(WorldKeys.LowHP, Comparision.GreaterThanOrEqual, 1)
            .AddCondition(WorldKeys.CanUsingSkill, Comparision.GreaterThanOrEqual, 1)
            .AddEffect(WorldKeys.UsedSkill, true);
    }

    public void AddUsedSkill(GoapSetBuilder builder)
    {
        builder.AddWorldSensor<UsedSkillSensor>().SetKey(WorldKeys.UsedSkill);
    }

    public void AddCanUseSkill(GoapSetBuilder builder)
    {
        builder.AddWorldSensor<CanUseSkillSensor>().SetKey(WorldKeys.CanUsingSkill);
    }

    public void AddTargetSkillSensor(GoapSetBuilder builder)
    {
        builder.AddTargetSensor<UsingSkillSensor>().SetTarget(Targets.SkillTarget);
    }

    public void AddLowHpSensor(GoapSetBuilder builder)
    {
        builder.AddWorldSensor<LiveSensor>().SetKey(WorldKeys.LowHP);
    }

    #endregion
}
