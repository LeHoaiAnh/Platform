using System.Collections.Generic;
using Goap.Classes;
using Goap.Classes.Runners;
using Goap.Interfaces;
using Goap.Resolver.Models;
using UnityEngine;

namespace Goap.Behaviours
{
    [DefaultExecutionOrder(-100)]
    public class GoapRunnerBehaviour : MonoBehaviour, IGoapRunner
    {
        private GoapRunner runner;

        public float RunTime => runner.RunTime;
        public float CompleteTime => runner.CompleteTime;
        public int RunCount { get; private set; }

        public GoapConfigInitializerBase configInitializer;
        public List<GoapSetFactoryBase> setConfigFactories = new();

        private GoapConfig config;

        private void Awake()
        {
           DoInit();
        }

        public void DoInit()
        {
            config = GoapConfig.Default;
            runner = new GoapRunner();
            
            if (configInitializer != null)
                configInitializer.InitConfig(config);
            
            CreateSets();
        }
        public void Register(IGoapSet set) => runner.Register(set);

        private void Update()
        {
            RunCount = runner.QueueCount;
            runner.Run();
        }

        private void LateUpdate()
        {
            runner.Complete();
        }
        
        private void OnDestroy()
        {
            runner.Dispose();
        }

        private void CreateSets()
        {
            var setFactory = new GoapSetFactory(config);
            
           setConfigFactories.ForEach(factory =>
            {
                Register(setFactory.Create(factory.Create()));
            });
        }

        public Graph GetGraph(IGoapSet set) => runner.GetGraph(set);
        public bool Knows(IGoapSet set) => runner.Knows(set);
        public IMonoAgent[] Agents => runner.Agents;
        public IGoapSet[] Sets => runner.Sets;
        public IGoapSet GetSet(string id) => runner.GetSet(id);
    }
}