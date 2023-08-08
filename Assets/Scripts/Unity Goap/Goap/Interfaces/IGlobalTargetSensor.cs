namespace Goap.Interfaces
{
    public interface IGlobalTargetSensor : ITargetSensor
    {
        public ITarget Sense();
    }
}