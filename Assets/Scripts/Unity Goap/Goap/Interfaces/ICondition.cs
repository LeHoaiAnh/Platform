using Goap.Configs.Interfaces;
using Goap.Resolver;

namespace Goap.Interfaces
{
    public interface ICondition : global::Goap.Resolver.Interfaces.ICondition {
        public IWorldKey WorldKey { get; }
        public Comparision comparision { get; }
        public int Amount { get; }
    }
}