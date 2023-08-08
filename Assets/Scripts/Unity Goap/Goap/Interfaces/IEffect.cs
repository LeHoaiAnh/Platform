using Goap.Configs.Interfaces;

namespace Goap.Interfaces
{
    public interface IEffect : global::Goap.Resolver.Interfaces.IEffect
    {
        public IWorldKey WorldKey { get; }
        public bool Increase { get; }
    }
}