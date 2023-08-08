using Goap.Classes;
using Goap.Configs.Interfaces;
using Goap.Interfaces;

namespace Goap.Sensors
{
    public abstract class GlobalWorldSensorBase : IGlobalWorldSensor
    {
        public IWorldKey Key => this.Config.Key;

        public IWorldSensorConfig Config { get; private set; }
        public void SetConfig(IWorldSensorConfig config) => this.Config = config;

        public abstract void Created();
        public abstract SenseValue Sense();
    }
}