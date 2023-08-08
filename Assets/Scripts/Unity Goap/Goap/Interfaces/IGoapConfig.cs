using Goap.Observers;

namespace Goap.Interfaces
{
    public interface IGoapConfig
    {
        IConditionObserver ConditionObserver { get; set; }
        IKeyResolver KeyResolver { get; set; }
        IGoapInjector GoapInjector { get; set; }
    }
}