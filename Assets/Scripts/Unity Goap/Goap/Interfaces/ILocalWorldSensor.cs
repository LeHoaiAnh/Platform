using Goap.Behaviours;
using Goap.Classes;
using Goap.Classes.References;

namespace Goap.Interfaces
{
    public interface ILocalWorldSensor : IWorldSensor
    {
        public void Update();
        public SenseValue Sense(IMonoAgent agent, IComponentReference references);
    }
}