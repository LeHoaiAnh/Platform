using System;
using System.Linq;
using Graph = Goap.Editor.Classes.Models.Graph;
using Node = Goap.Editor.Classes.Models.Node;

namespace Goap.Editor.Classes
{
    public class DebugGraph
    {
        private readonly Graph graph;

        public DebugGraph(Graph graph)
        {
            this.graph = graph;
        }

        public Nodes GetGraph(Models.Node entryNode)
        {
            var nodes = new Nodes();
            
            this.StoreNode(0, entryNode, nodes);

            return nodes;
        }

        private void StoreNode(int depth, Models.Node node, Nodes nodes)
        {
            nodes.Add(depth, node);

            foreach (var connection in node.Conditions.SelectMany(condition => condition.Connections))
            {
                this.StoreNode(depth + 1, this.GetNode(connection), nodes);
            }
        }

        private Models.Node GetNode(Guid guid)
        {
            return this.graph.AllNodes.Values.First(x => x.Guid == guid);
        }
    }
}