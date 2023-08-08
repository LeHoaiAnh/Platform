using Goap.Configs.Interfaces;
using Goap.Interfaces;

namespace Goap.Classes
{
    public class Effect : IEffect
    {
        public IWorldKey WorldKey { get; set; }
        public bool Increase { get; set; }
    }
}