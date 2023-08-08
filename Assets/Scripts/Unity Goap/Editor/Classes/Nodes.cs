using System;
using System.Collections.Generic;
using System.Linq;
using Goap.Editor.Classes.Models;

namespace Goap.Editor.Classes
{
    public class Nodes
    {
        public Dictionary<int, List<RenderNode>> DepthNodes { get; private set; } = new ();
        public Dictionary<Guid, RenderNode> AllNodes { get; private set; } = new();
        public int MaxWidth { get; private set; }

        public RenderNode Get(Guid guid) => this.AllNodes[guid];
        
        private List<RenderNode> GetList(int depth)
        {
            if (this.DepthNodes.TryGetValue(depth, out var levels))
                return levels;

            levels = new List<RenderNode>();
            this.DepthNodes.Add(depth, levels);
            return levels;
        }
        
        public int GetMaxWidth()
        {
            var max = 0;

            foreach (var (key, value) in this.DepthNodes)
            {
                if (value.Count > max)
                    max = value.Count;
            }

            return max;
        }

        public void Add(int depth, Node node)
        {
            if (AllNodes.Values.Any(x => x.Node == node))
            {
                RenderNode existNode = AllNodes.Values.First(e => e.Node == node);
                if (depth > existNode.Depth)
                {
                    GetList(existNode.Depth).Remove(existNode);
                    existNode.Depth = depth;
                    GetList(depth).Add(existNode);

                }
                return;
            }

            
            var newNode = new RenderNode(node, depth);
            this.GetList(depth).Add(newNode);
            this.AllNodes.Add(newNode.Node.Guid, newNode);

            this.MaxWidth = this.GetMaxWidth();
        }
    }
}