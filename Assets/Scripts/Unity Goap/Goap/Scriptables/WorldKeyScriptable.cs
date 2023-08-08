using Goap.Configs.Interfaces;
using UnityEngine;

namespace Goap.Scriptables
{
    [CreateAssetMenu(menuName = "Goap/WorldKey")]
    public class WorldKeyScriptable : ScriptableObject, IWorldKey
    {
        public string Name => name;
    }
}