using System.Collections.Generic;
using Goap.Configs.Interfaces;
using Goap.Enums;
using Goap.Resolver;

namespace Goap.Interfaces
{
    public interface IWorldData
    {
        public Dictionary<IWorldKey, int> States { get; }
        public Dictionary<ITargetKey, ITarget> Targets { get; }
        public ITarget GetTarget(IActionBase action);
        void SetState(IWorldKey key, int state);
        void SetTarget(ITargetKey key, ITarget target);
        public bool IsTrue(IWorldKey worldKey, Comparision comparision, int value);
    }
}