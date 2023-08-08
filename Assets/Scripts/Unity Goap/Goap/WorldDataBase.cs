using System.Collections.Generic;
using Goap.Configs.Interfaces;
using Goap.Interfaces;
using Goap.Resolver;

namespace Goap
{
    public abstract class WorldDataBase : IWorldData
    {
        public Dictionary<IWorldKey, int> States { get; } = new();
        public Dictionary<ITargetKey, ITarget> Targets { get; } = new();

        public ITarget GetTarget(IActionBase action)
        {
            if (action.Config.Target == null)
                return null;
            
            if (!this.Targets.ContainsKey(action.Config.Target))
                return null;
            
            return this.Targets[action.Config.Target];
        }

        public bool IsTrue(IWorldKey worldKey, Comparision comparision, int value)
        {
            if (!this.States.ContainsKey(worldKey))
                return false;
            
            var state = this.States[worldKey];

            switch (comparision)
            {
                case Comparision.GreaterThan:
                    return state > value;
                case Comparision.GreaterThanOrEqual:
                    return state >= value;
                case Comparision.SmallerThan:
                    return state < value;
                case Comparision.SmallerThanOrEqual:
                    return state <= value;
            }

            return false;
        }

        public void SetState(IWorldKey key, int state)
        {
            if (key == null)
                return;
            
            if (this.States.ContainsKey(key))
            {
                this.States[key] = state;
                return;
            }
            
            this.States.Add(key, state);
        }
        
        public void SetTarget(ITargetKey key, ITarget target)
        {
            if (key == null)
                return;
            
            if (this.Targets.ContainsKey(key))
            {
                this.Targets[key] = target;
                return;
            }
            
            this.Targets.Add(key, target);
        }
    }
}