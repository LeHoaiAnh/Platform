using System;
using Goap.Resolver.Interfaces;

namespace Goap.Resolver.Models
{
    public class NodeCondition
    {
        public ICondition Condition { get; set; }
        public Node[] Connections { get; set; } = Array.Empty<Node>();
    }
}