using Goap;

namespace Goap.Interfaces
{
    public interface ISensorRunner
    {
        void Update();
        GlobalWorldData SenseGlobal();
        LocalWorldData SenseLocal(GlobalWorldData worldData, IMonoAgent agent);
    }
}