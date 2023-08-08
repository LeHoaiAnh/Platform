using System.Linq;
using Goap.Classes.Validators;
using Goap.Editor.Classes;
using Goap.Interfaces;
using Goap.Resolver;
using UnityEngine.UIElements;

namespace Goap.Editor.NodeViewer.Drawers
{
    public class NodeDrawer : Box
    {
        public NodeDrawer(RenderNode node, IAgent agent)
        {
            Clear();
            
            name = "node-viewer__node";
            
            AddToClassList(this.GetClass(node, agent));

            style.width = node.Rect.width;
            // this.style.height = node.Rect.height;

            transform.position = node.Position;
            Add(new Label(node.Node.Action.GetType().GetGenericTypeName())
            {
                name = "node-viewer__node__label"
            });
            
            RenderAction(agent, node.Node.Action as IActionBase);
            RenderGoal(agent, node.Node.Action as IGoalBase);
        }

        private string GetClass(RenderNode node, IAgent agent)
        {
            if (agent.CurrentGoal == node.Node.Action)
                return "node-viewer__node--path";
            
            if (agent.CurrentAction == node.Node.Action)
                return "node-viewer__node--active";
            
            if (agent.CurrentActionPath.Contains(node.Node.Action))
                return "node-viewer__node--path";
            
            return "node-viewer__node--normal";
        }

        private void RenderGoal(IAgent agent, IGoalBase goal)
        {
            if (goal == null)
                return;

            var conditionObserver = agent.GoapSet.GoapConfig.ConditionObserver;
            conditionObserver.SetWorldData(agent.WorldData);

            var conditions = goal.Conditions.Select(x => this.GetText(x as ICondition, conditionObserver.IsMet(x)));

            var text = $"<b>Conditions:</b>\n{string.Join("\n", conditions)}";
            var intensityText = $"<b>Intensity:</b>\n{goal.GetIntensity()}";
            
            Add(new Label(text));
            Add(new Label(intensityText));
        }

        private void RenderAction(IAgent agent, IActionBase action)
        {
            if (action == null)
                return;

            if (agent.WorldData == null)
                return;

            var conditionObserver = agent.GoapSet.GoapConfig.ConditionObserver;
            conditionObserver.SetWorldData(agent.WorldData);

            var conditions = action.Conditions.Select(x => this.GetText(x as ICondition, conditionObserver.IsMet(x)));
            var effects = action.Effects.Select(x => this.GetText(x as IEffect));

            var cost = action.GetCost(agent as IMonoAgent, agent.Injector);
            
            var target = agent.WorldData.GetTarget(action);

            var text = $"<b>Cost:</b> {cost}\n<b>Target:</b>\n";
            
            if (target != null)
                text += $"    {action.Config.Target.Name}\n    {target.Position}\n";

            text += $"\n<b>Effects</b>:\n{string.Join("\n", effects)}\n<b>Conditions</b>:\n{string.Join("\n", conditions)}";
            
            this.Add(new Label(text));
        }
        
        private string GetText(ICondition condition, bool value)
        {
            var color = value ? "green" : "red";

            return $"    <color={color}>{condition.WorldKey.Name} {GetText(condition.comparision)} {condition.Amount}</color>";
        }

        private string GetText(IEffect effect)
        {
            var suffix = effect.Increase ? "++" : "--";

            return $"    {effect.WorldKey.Name}{suffix}";
        }

        private string GetText(Comparision comparison)
        {
            switch (comparison)
            {
                case Comparision.GreaterThan:
                    return ">";
                case Comparision.GreaterThanOrEqual:
                    return ">=";
                case Comparision.SmallerThan:
                    return "<";
                case Comparision.SmallerThanOrEqual:
                    return "<=";
            }
            
            return "";
        }
    }
}