using Goap.Configs.Interfaces;
using Goap.Resolver.Interfaces;

namespace Goap.Interfaces
{
    public interface IGoalBase : IAction, IHasConfig<IGoalConfig>
    {
        public int GetIntensity();
        public int GetCost(IWorldData data);
    }
}