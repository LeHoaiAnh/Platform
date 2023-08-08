using System;
using System.Linq;
using Goap.Configs.Interfaces;
using Goap.Interfaces;

namespace Goap.Behaviours
{
    public abstract class GoalBase : IGoalBase
    {
        private IGoalConfig config;
        public IGoalConfig Config => config;
        
        public Guid Guid { get; } = Guid.NewGuid();
        public global::Goap.Resolver.Interfaces.IEffect[] Effects { get; } = {};
        public global::Goap.Resolver.Interfaces.ICondition[] Conditions => config.Conditions.Cast<global::Goap.Resolver.Interfaces.ICondition>().ToArray();

        public void SetConfig(IGoalConfig config)
        {
            this.config = config;
        }

        public virtual int GetCost(IWorldData data)
        {
            return config.BaseCost;
        }

        public virtual int GetIntensity()
        {
            return config.Intensity;
        }
    }
}