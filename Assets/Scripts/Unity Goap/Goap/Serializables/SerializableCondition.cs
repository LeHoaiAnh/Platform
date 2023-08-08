using System;
using Goap.Configs.Interfaces;
using Goap.Interfaces;
using Goap.Resolver;
using Goap.Scriptables;
using UnityEngine;

namespace Goap.Serializables
{
    [Serializable]
    public class SerializableCondition : ICondition
    {
        public WorldKeyScriptable worldKey;

        public IWorldKey WorldKey => this.worldKey;
        
        [field:SerializeField]
        public Comparision comparision { get; set; }
        
        [field:SerializeField]
        public int Amount { get; set; }
    }
}