using System.Collections.Generic;
using Goap.Behaviours;
using Goap.Classes;
using Goap.Resolver.Interfaces;

namespace Goap.Interfaces
{
    public interface IGoapSet
    {
        string Id { get; }
        IGoapConfig GoapConfig { get; }
        IAgentCollection Agents { get; }
        ISensorRunner SensorRunner { get; }
        void Register(IMonoAgent agent);
        void Unregister(IMonoAgent agent);
        List<IAction> GetAllNodes();
        List<IActionBase> GetActions();

        public void SortGoal();
        public List<IGoalBase> goals { get; set; }

        TAction ResolveAction<TAction>()
            where TAction : ActionBase;

        TGoal ResolveGoal<TGoal>() where TGoal : IGoalBase;

        AgentDebugGraph GetDebugGraph();
    }
}