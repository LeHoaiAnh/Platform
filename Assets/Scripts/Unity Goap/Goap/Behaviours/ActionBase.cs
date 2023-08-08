using System;
using System.Linq;
using Goap.Classes;
using Goap.Classes.References;
using Goap.Configs.Interfaces;
using Goap.Enums;
using Goap.Interfaces;
using UnityEngine;
using ICondition = Goap.Resolver.Interfaces.ICondition;
using IEffect = Goap.Resolver.Interfaces.IEffect;

namespace Goap.Behaviours
{
    public abstract class ActionBase<TActionData> : ActionBase
        where TActionData : IActionData, new()
    {
        public override IActionData GetData()
        {
            return CreateData();
        }

        public virtual TActionData CreateData()
        {
            return new TActionData();
        }

        public override void Start(IMonoAgent agent, IActionData data) => this.Start(agent, (TActionData) data);
        
        public abstract void Start(IMonoAgent agent, TActionData data);

        public override ActionRunState Perform(IMonoAgent agent, IActionData data, float deltaTime) => this.Perform(agent, (TActionData) data, deltaTime);

        public abstract ActionRunState Perform(IMonoAgent agent, TActionData data, float deltaTime);

        public override void End(IMonoAgent agent, IActionData data) => this.End(agent, (TActionData) data);
        
        public abstract void End(IMonoAgent agent, TActionData data);
    }

    public abstract class ActionBase : IActionBase
    {
        private IActionConfig config;
        
        public IActionConfig Config => config;
        
        public Guid Guid { get; } = Guid.NewGuid();
        public IEffect[] Effects => this.config.Effects.Cast<IEffect>().ToArray();
        public ICondition[] Conditions => this.config.Conditions.Cast<ICondition>().ToArray();

        public void SetConfig(IActionConfig config)
        {
            this.config = config;
        }

        public virtual float GetCost(IMonoAgent agent, IComponentReference references)
        {
            return config.BaseCost;
        }
        
        public virtual float GetInRange(IMonoAgent agent, IActionData data)
        {
            return config.InRange;
        }

        public abstract IActionData GetData();
        public abstract void Created();
        public abstract ActionRunState Perform(IMonoAgent agent, IActionData data, float deltaTime);
        public abstract void Start(IMonoAgent agent, IActionData data);
        public abstract void End(IMonoAgent agent, IActionData data);
    }
}