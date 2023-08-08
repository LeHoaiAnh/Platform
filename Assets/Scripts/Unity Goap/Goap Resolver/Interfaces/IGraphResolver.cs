using Goap.Resolver.Models;

namespace Goap.Resolver.Interfaces
{
    public interface IGraphResolver
    {
        IResolveHandle StartResolve(RunData runData);
        IExecutableBuilder GetExecutableBuilder();
        IPositionBuilder GetPositionBuilder();
        ICostBuilder GetCostBuilder();
        Graph GetGraph();
        int GetIndex(IAction action);
        IAction GetAction(int index);
        void Dispose();
    }
}