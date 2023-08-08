using System;
using Goap.Resolver.Interfaces;

namespace Goap.Resolver.Models
{
    public class NodeEffect
    {
        public IEffect Effect { get; set; }
        public Node[] Connections { get; set; } = Array.Empty<Node>();
    }
}