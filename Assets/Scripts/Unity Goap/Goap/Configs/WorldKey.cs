using Goap.Configs.Interfaces;

namespace Goap.Configs
{
    public class WorldKey : IWorldKey
    {
        public WorldKey(string name)
        {
            this.Name = name;
        }
        
        public string Name { get; }
    }
}