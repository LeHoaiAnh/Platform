using Goap.Interfaces;

namespace Goap.Behaviours
{
    public class AgentEvents : IAgentEvents
    {
        public event ActionDelegate OnActionStart;
        public void ActionStart(IActionBase action)
        {
            OnActionStart?.Invoke(action);
        }
        
        public event ActionDelegate OnActionStop;
        public void ActionStop(IActionBase action)
        {
            OnActionStop?.Invoke(action);
        }
        
        public event GoalDelegate OnGoalStart;
        public void GoalStart(IGoalBase goal)
        {
            OnGoalStart?.Invoke(goal);
        }
        
        public event GoalDelegate OnGoalCompleted;
        public void GoalCompleted(IGoalBase goal)
        {
            OnGoalCompleted?.Invoke(goal);
        }
        
        public event GoalDelegate OnNoActionFound;
        public void NoActionFound(IGoalBase goal)
        {
            OnNoActionFound?.Invoke(goal);
        }
    }
}