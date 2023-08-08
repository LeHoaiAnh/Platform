using System.Collections.Generic;
using System.Linq;
using Goap.Attributes;
using Goap.Configs.Interfaces;
using Goap.Interfaces;
using Goap.Serializables;
using UnityEngine;

namespace Goap.Scriptables
{
    [CreateAssetMenu(menuName = "Goap/GoalConfig")]
    public class GoalConfigScriptable : ScriptableObject, IGoalConfig
    {
        [GoalClass]
        public string classType;
        public int baseCost = 1;
        public int intensity = 1;
        public List<SerializableCondition> conditions;

        public string Name => this.name;

        public List<ICondition> Conditions => this.conditions.Cast<ICondition>().ToList();

        public int BaseCost
        {
            get => baseCost;
            set => baseCost = value;
        }

        public int Intensity
        {
            get => intensity;
            set => intensity = value;
        }

        public string ClassType
        {
            get => classType;
            set => classType = value;
        }
    }
}