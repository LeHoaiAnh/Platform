
using Goap.Resolver.Interfaces;

namespace Goap.Interfaces
{
    public interface IKeyResolver : IActionKeyResolver
    {
        void SetWorldData(IWorldData globalWorldData);
    }
}