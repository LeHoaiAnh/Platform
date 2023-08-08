using System.Collections.Generic;
using Goap.Interfaces;

namespace Goap.Configs.Interfaces
{
    public interface IGoalConfig : IClassConfig
    {
        int BaseCost { get; set; }
        int Intensity { get; set; }
        List<ICondition> Conditions { get; }
    }
}