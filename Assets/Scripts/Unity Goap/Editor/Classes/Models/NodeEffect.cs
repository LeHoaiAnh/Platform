using System;
using Goap.Resolver.Interfaces;

namespace Goap.Editor.Classes.Models
{
    public class NodeEffect
    {
        public IEffect Effect { get; set; }
        public Guid[] Connections { get; set; } = {};
    }
}