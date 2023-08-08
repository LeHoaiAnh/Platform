using Goap.Classes;
using Goap.Classes.References;
using Goap.Configs.Interfaces;
using Goap.Enums;
using Goap.Resolver.Interfaces;

namespace Goap.Interfaces
{
    public interface IActionBase : IAction, IHasConfig<IActionConfig>
    {
        float GetCost(IMonoAgent agent, IComponentReference references);
        float GetInRange(IMonoAgent agent, IActionData data);
        IActionData GetData();
        void Created();
        public ActionRunState Perform(IMonoAgent agent, IActionData data, float deltaTime);
        void Start(IMonoAgent agent, IActionData data);
        void End(IMonoAgent agent, IActionData data);
    }
}