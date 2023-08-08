using Goap.Behaviours;
using Goap.Resolver.Models;

namespace Goap.Interfaces
{
    public interface IGoapRunner
    {
        void Register(IGoapSet set);
        Graph GetGraph(IGoapSet set);
        bool Knows(IGoapSet set);
        IMonoAgent[] Agents { get; }
        IGoapSet[] Sets { get; }
        IGoapSet GetSet(string id);
    }
}