using Goap.Classes;
using Goap.Classes.References;
using Goap.Configs.Interfaces;
using Goap.Interfaces;

namespace Goap.Sensors
{
    public abstract class LocalWorldSensorBase : ILocalWorldSensor
    {
        public IWorldKey Key => this.Config.Key;

        public IWorldSensorConfig Config { get; private set; }
        public void SetConfig(IWorldSensorConfig config) => this.Config = config;

        public abstract void Created();
        public abstract void Update();
        public abstract SenseValue Sense(IMonoAgent agent, IComponentReference references);
    }
}