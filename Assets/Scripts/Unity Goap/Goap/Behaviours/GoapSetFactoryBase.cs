using Goap.Configs;
using Goap.Configs.Interfaces;
using Goap.Interfaces;
using UnityEngine;

namespace Goap.Behaviours
{
    public abstract class GoapSetFactoryBase : MonoBehaviour
    {
        public abstract IGoapSetConfig Create();
    }
}