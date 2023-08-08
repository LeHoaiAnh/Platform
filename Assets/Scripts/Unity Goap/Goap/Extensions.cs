using Goap.Interfaces;
using UnityEngine;

namespace Goap
{
    public static class Extensions
    {
        public static bool IsNull(this IMonoAgent agent)
            => agent is MonoBehaviour mono && mono == null;
    }
}