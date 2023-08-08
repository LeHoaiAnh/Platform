using System;
using Goap.Configs.Interfaces;
using Goap.Interfaces;
using Goap.Scriptables;
using UnityEngine;

namespace Goap.Serializables
{
    [Serializable]
    public class SerializableEffect : IEffect
    {
        public WorldKeyScriptable worldKey;

        public IWorldKey WorldKey => this.worldKey;
        
        [field:SerializeField]
        public bool Increase { get; set; }
    }
}