using System;
using System.Collections.Generic;
using Goap.Configs.Interfaces;
using Goap.Interfaces;

namespace Goap.Configs
{
    [Serializable]
    public class GoalConfig : IGoalConfig
    {
        public GoalConfig(Type type)
        {
            this.Name = type.Name;
            this.ClassType = type.AssemblyQualifiedName;
        }

        public string Name { get; }
        public string ClassType { get; set; }
        public int BaseCost { get; set; }
        
        public int Intensity { get; set; }
        public List<ICondition> Conditions { get; set; } = new();
        
        public static GoalConfig Create<TGoal>()
            where TGoal : IGoalBase
        {
            return new GoalConfig(typeof(TGoal));
        }
    }
}