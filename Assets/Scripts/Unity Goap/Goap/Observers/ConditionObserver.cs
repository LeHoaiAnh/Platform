using Goap.Interfaces;

namespace Goap.Observers
{
    public class ConditionObserver : ConditionObserverBase
    {
        public override bool IsMet(ICondition condition)
        {
            return this.WorldData.IsTrue(condition.WorldKey, condition.comparision, condition.Amount);
        }
    }
}