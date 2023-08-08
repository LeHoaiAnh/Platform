using Goap.Classes;
using Goap.Interfaces;
using Goap.Scriptables;
using UnityEngine;

namespace Goap.Behaviours
{
    [DefaultExecutionOrder(-99)]
    public class GoapSetBehaviour : MonoBehaviour
    {
        [SerializeField]
        private GoapSetConfigScriptable config;

        [SerializeField]
        private GoapRunnerBehaviour runner;

        public IGoapSet Set { get; private set; }

        private void Awake()
        {
            var set = new GoapSetFactory(GoapConfig.Default).Create(config);

            runner.Register(set);
            
            Set = set;
        }
    }
}