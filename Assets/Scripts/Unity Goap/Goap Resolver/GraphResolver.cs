using System.Collections.Generic;
using System.Linq;
using Goap.Resolver.Interfaces;
using Goap.Resolver.Models;
using Unity.Collections;
using Unity.Jobs;
using IAction = Goap.Resolver.Interfaces.IAction;

namespace Goap.Resolver
{
    public class GraphResolver : IGraphResolver
    {
        private readonly List<Node> indexList;
        private readonly List<IAction> actionIndexList;
        private NativeMultiHashMap<int, int> map;
        
        private GraphResolverJob job;
        //private JobHandle handle;

        private Graph graph;

        public GraphResolver(IAction[] actions, IActionKeyResolver keyResolver)
        {
            graph = new GraphBuilder(keyResolver).Build(actions);
            
            indexList = graph.AllNodes.ToList();
            actionIndexList = indexList.Select(x => x.Action).ToList();

            var map = new NativeMultiHashMap<int, int>(indexList.Count, Allocator.Persistent);
            
            for (var i = 0; i < indexList.Count; i++)
            {
                var connections = indexList[i].Conditions
                    .SelectMany(x => x.Connections.Select(y => this.indexList.IndexOf(y)));

                foreach (var connection in connections)
                {
                    map.Add(i, connection);
                }
            }

            this.map = map;
        }

        public IResolveHandle StartResolve(RunData runData)
        {
            return new ResolveHandle(this, map, runData);
        }
        
        public IExecutableBuilder GetExecutableBuilder()
        {
            return new ExecutableBuilder(actionIndexList);
        }
        
        public IPositionBuilder GetPositionBuilder()
        {
            return new PositionBuilder(actionIndexList);
        }

        public ICostBuilder GetCostBuilder()
        {
            return new CostBuilder(actionIndexList);
        }
        
        public Graph GetGraph()
        {
            return graph;
        }
        
        public int GetIndex(IAction action) => actionIndexList.IndexOf(action);
        public IAction GetAction(int index) => actionIndexList[index];
        
        public void Dispose()
        {
            map.Dispose();
        }
    }
}