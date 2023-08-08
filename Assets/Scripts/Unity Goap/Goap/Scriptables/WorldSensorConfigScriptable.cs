using Goap.Attributes;
using Goap.Configs.Interfaces;
using UnityEngine;

namespace Goap.Scriptables
{
    [CreateAssetMenu(menuName = "Goap/WorldSensorConfig")]
    public class WorldSensorConfigScriptable : ScriptableObject, IWorldSensorConfig
    {
        [WorldSensor]
        public string classType;

        public WorldKeyScriptable key;

        public string Name => this.name;

        public string ClassType
        {
            get => this.classType;
            set => this.classType = value;
        }

        public IWorldKey Key => this.key;
    }
}