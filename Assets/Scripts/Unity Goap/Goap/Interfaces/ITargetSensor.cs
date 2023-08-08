using Goap.Configs.Interfaces;

namespace Goap.Interfaces
{
    public interface ITargetSensor : IHasConfig<ITargetSensorConfig>
    {
        public ITargetKey Key { get; }
        void Created();
    }
}