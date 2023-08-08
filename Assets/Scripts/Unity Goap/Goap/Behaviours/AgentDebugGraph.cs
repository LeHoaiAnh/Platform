using System.Collections.Generic;
using Goap.Classes;
using Goap.Interfaces;

namespace Goap.Behaviours
{
    public struct AgentDebugGraph
    {
        public List<IGoalBase> Goals { get; set; }
        public List<IActionBase> Actions { get; set; }
        public IGoapConfig Config { get; set; }
    }
}