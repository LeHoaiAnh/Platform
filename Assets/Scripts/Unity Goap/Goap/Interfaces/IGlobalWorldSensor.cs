using Goap.Classes;

namespace Goap.Interfaces
{
    public interface IGlobalWorldSensor : IWorldSensor
    {
        public SenseValue Sense();
    }
}