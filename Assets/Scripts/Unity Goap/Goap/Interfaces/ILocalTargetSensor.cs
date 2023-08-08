using Goap.Behaviours;
using Goap.Classes.References;

namespace Goap.Interfaces
{
    public interface ILocalTargetSensor : ITargetSensor
    {
        public void Update();
        
        public ITarget Sense(IMonoAgent agent, IComponentReference references);
    }
}