using Goap.Interfaces;
using UnityEngine;

namespace Goap.Classes
{
    public class PositionTarget : ITarget
    {
        public Vector3 Position { get; }

        public PositionTarget(Vector3 position)
        {
            Position = position;
        }
    }
}