using System;
using System.Collections.Generic;
using Goap.Classes;
using Goap.Configs;
using Goap.Configs.Interfaces;
using Goap.Interfaces;
using Goap.Resolver;

namespace Goap.Classes.Builders
{
    public class GoalBuilder
    {
        private readonly GoalConfig config;
        private readonly List<ICondition> conditions = new();
        private readonly WorldKeyBuilder worldKeyBuilder;

        public GoalBuilder(Type type, WorldKeyBuilder worldKeyBuilder)
        {
            this.worldKeyBuilder = worldKeyBuilder;
            config = new GoalConfig(type)
            {
                BaseCost = 1,
                ClassType = type.AssemblyQualifiedName
            };
        }
        
        public GoalBuilder SetBaseCost(int baseCost)
        {
            config.BaseCost = baseCost;
            return this;
        }
        
        public GoalBuilder AddCondition(string key, Comparision comparision, int amount)
        {
            conditions.Add(new Condition(worldKeyBuilder.GetKey(key), comparision, amount));
            return this;
        }
        
        public GoalBuilder AddCondition<T>(string key, Comparision comparision, int amount)
        {
            conditions.Add(new Condition(worldKeyBuilder.GetKey<T>(key), comparision, amount));
            return this;
        }
        
        public IGoalConfig Build()
        {
            config.Conditions = conditions;
            return config;
        }
        
        public static GoalBuilder Create<TGoal>(WorldKeyBuilder worldKeyBuilder)
            where TGoal : IGoalBase
        {
            return new GoalBuilder(typeof(TGoal), worldKeyBuilder);
        }
    }
}