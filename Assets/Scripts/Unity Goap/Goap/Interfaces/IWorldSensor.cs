using Goap.Configs;
using Goap.Configs.Interfaces;

namespace Goap.Interfaces
{
    public interface IWorldSensor : IHasConfig<IWorldSensorConfig>
    {
        public IWorldKey Key { get; }
        void Created();
    }
}