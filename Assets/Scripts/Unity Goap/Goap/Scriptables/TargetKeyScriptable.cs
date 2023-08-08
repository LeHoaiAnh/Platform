using Goap.Configs.Interfaces;
using Goap.Resolver.Interfaces;
using UnityEngine;

namespace Goap.Scriptables
{
    [CreateAssetMenu(menuName = "Goap/TargetKey")]
    public class TargetKeyScriptable : ScriptableObject, ITargetKey, IEffect, ICondition
    {
        public string Name => this.name;
    }
}