﻿using System.Collections.Generic;
using System.Linq;
using Goap.Configs.Interfaces;
using UnityEngine;

namespace Goap.Scriptables
{
    [CreateAssetMenu(menuName = "Goap/GoapSetConfig")]
    public class GoapSetConfigScriptable : ScriptableObject, IGoapSetConfig
    {
        public List<ActionConfigScriptable> actions = new List<ActionConfigScriptable>();
        public List<GoalConfigScriptable> goals = new List<GoalConfigScriptable>();

        public List<TargetSensorConfigScriptable> targetSensors = new List<TargetSensorConfigScriptable>();
        public List<WorldSensorConfigScriptable> worldSensors = new List<WorldSensorConfigScriptable>();

        public string Name => name;
        public List<IActionConfig> Actions => actions.Cast<IActionConfig>().ToList();
        public List<IGoalConfig> Goals => goals.Cast<IGoalConfig>().ToList();
        public List<ITargetSensorConfig> TargetSensors => this.targetSensors.Cast<ITargetSensorConfig>().ToList();
        public List<IWorldSensorConfig> WorldSensors => this.worldSensors.Cast<IWorldSensorConfig>().ToList();
    }
}