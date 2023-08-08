using Goap.Classes;
using UnityEngine;

namespace Goap.Behaviours
{
    public abstract class GoapConfigInitializerBase : MonoBehaviour
    {
        public abstract void InitConfig(GoapConfig config);
    }
}