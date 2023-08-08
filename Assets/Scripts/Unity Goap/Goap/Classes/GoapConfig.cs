﻿using Goap.Classes.Injectors;
using Goap.Interfaces;
using Goap.Observers;
using Goap.Resolvers;

namespace Goap.Classes
{
    public class GoapConfig: IGoapConfig
    {
        public IConditionObserver ConditionObserver { get; set; }
        public IKeyResolver KeyResolver { get; set; }
        public IGoapInjector GoapInjector { get; set; }
        
        public static GoapConfig Default => new GoapConfig
        {
            ConditionObserver = new ConditionObserver(),
            KeyResolver = new KeyResolver(),
            GoapInjector = new GoapInjector()
        };
    }
}