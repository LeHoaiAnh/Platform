using Goap.Classes.References;
using Goap.Configs.Interfaces;
using Goap.Interfaces;

namespace Goap.Sensors
{
    public abstract class LocalTargetSensorBase : ILocalTargetSensor
    {
        public ITargetKey Key => this.Config.Key;
        public ITargetSensorConfig Config { get; private set; }
        public void SetConfig(ITargetSensorConfig config) => this.Config = config;
        
        public abstract void Created();
        public abstract void Update();
        public abstract ITarget Sense(IMonoAgent agent, IComponentReference references);
    }
}