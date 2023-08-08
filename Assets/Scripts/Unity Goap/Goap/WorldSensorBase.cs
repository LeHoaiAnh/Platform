using Goap.Configs.Interfaces;
using UnityEngine;

namespace Goap
{
    public abstract class WorldSensorBase : MonoBehaviour
    {
        [SerializeField]
        private IWorldKey keyScriptable;

        public IWorldKey KeyScriptable => this.keyScriptable;
    }
}