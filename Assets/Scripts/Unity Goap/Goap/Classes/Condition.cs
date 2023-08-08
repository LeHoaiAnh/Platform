using Goap.Configs.Interfaces;
using Goap.Interfaces;
using Goap.Resolver;

namespace Goap.Classes
{
    public class Condition : ICondition
    {
        public IWorldKey WorldKey { get; set; }
        public Comparision comparision { get; set; }
        public int Amount { get; set; }

        public Condition()
        {
        }

        public Condition(IWorldKey worldKey, Comparision comparision, int amount)
        {
            this.WorldKey = worldKey;
            this.comparision = comparision;
            this.Amount = amount;
        }
    }
}