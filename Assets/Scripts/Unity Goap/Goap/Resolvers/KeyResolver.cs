using System;
using Goap.Interfaces;
using Goap.Resolver;

namespace Goap.Resolvers
{
    public class KeyResolver : KeyResolverBase
    {
        protected override string GetKey(IActionBase action, ICondition condition)
        {
            return condition.WorldKey.Name + this.GetText(condition.comparision);
        }

        protected override string GetKey(IActionBase action, IEffect effect)
        {
            return effect.WorldKey.Name + this.GetText(effect.Increase);
        }

        protected override string GetKey(IGoalBase action, ICondition condition)
        {
            return condition.WorldKey.Name + this.GetText(condition.comparision);
        }

        private string GetText(bool value)
        {
            if (value)
                return "_increase";

            return "_decrease";
        }

        private string GetText(Comparision comparision)
        {
            switch (comparision)
            {
                case Comparision.GreaterThan:
                case Comparision.GreaterThanOrEqual:
                    return "_increase";
                case Comparision.SmallerThan:
                case Comparision.SmallerThanOrEqual:
                    return "_decrease";
            }

            throw new Exception($"Comparision type {comparision} not supported");
        }
    }
}