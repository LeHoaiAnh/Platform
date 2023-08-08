using System.Collections.Generic;
using System.Linq;
using Goap.Behaviours;
using Goap.Classes.Runners;
using Goap.Interfaces;
using Goap.Resolver.Interfaces;

namespace Goap.Classes
{
    public class GoapSet : IGoapSet
    {
        public string Id { get; }
        public IAgentCollection Agents { get; } = new AgentCollection();
        public IGoapConfig GoapConfig { get; }
        public ISensorRunner SensorRunner { get; }
        
        public List<IGoalBase> goals { get; set; }
        private List<IActionBase> actions;

        public GoapSet(string id, IGoapConfig config, List<IGoalBase> goals, List<IActionBase> actions, SensorRunner sensorRunner)
        {
            this.Id = id;
            this.GoapConfig = config;
            this.SensorRunner = sensorRunner;
            this.goals = goals;
            this.actions = actions;
        }

        public void Register(IMonoAgent agent) => this.Agents.Add(agent);
        public void Unregister(IMonoAgent agent) => this.Agents.Remove(agent);

        public TAction ResolveAction<TAction>() where TAction : ActionBase
        {
            var result = this.actions.FirstOrDefault(x => x.GetType() == typeof(TAction));

            if (result != null)
                return (TAction) result;
            
            throw new KeyNotFoundException($"No action found of type {typeof(TAction)}");
        }
        
        public void SortGoal()
        {
            goals.Sort((e1, e2) =>
            {
                if (e2.GetIntensity() > e1.GetIntensity())
                {
                    return 1;
                }
                else if (e2.GetIntensity() < e1.GetIntensity())
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            });
        }
        
        public TGoal ResolveGoal<TGoal>() where TGoal : IGoalBase
        {
            var result = this.goals.FirstOrDefault(x => x.GetType() == typeof(TGoal));

            if (result != null)
                return (TGoal) result;
            
            throw new KeyNotFoundException($"No Goal found of type {typeof(TGoal)}");
        }

        public List<IAction> GetAllNodes() => this.actions.Cast<IAction>().Concat(this.goals).ToList();
        public List<IActionBase> GetActions() => this.actions;

        public AgentDebugGraph GetDebugGraph()
        {
            return new AgentDebugGraph
            {
                Goals = this.goals,
                Actions = this.actions,
                Config = this.GoapConfig
            };
        }
    }
}