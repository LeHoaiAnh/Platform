using System.Collections.Generic;
using Goap.Configs;
using Goap.Configs.Interfaces;
using Goap.Exceptions;
using Goap.Interfaces;
using Goap.Resolvers;
using Goap.Classes.Runners;
using Goap.Classes.Validators;
using UnityEngine;

namespace Goap.Classes
{
    public class GoapSetFactory
    {
        private readonly IGoapConfig goapConfig;
        private IGoapSetConfigValidatorRunner goapSetConfigValidatorRunner = new GoapSetConfigValidatorRunner();

        public GoapSetFactory(IGoapConfig goapConfig)
        {
            this.goapConfig = goapConfig;
        }
        
        public GoapSet Create(IGoapSetConfig config)
        {
            this.Validate(config);
            
            var sensorRunner = this.CreateSensorRunner(config);

            return new GoapSet(
                id: config.Name,
                config: this.goapConfig,
                actions: this.GetActions(config),
                goals: this.GetGoals(config),
                sensorRunner: sensorRunner
            );
        }
        
        private void Validate(IGoapSetConfig config)
        {
            var results = this.goapSetConfigValidatorRunner.Validate(config);

            foreach (var error in results.GetErrors())
            {
                Debug.LogError(error);
            }
            
            foreach (var warning in results.GetWarnings())
            {
                Debug.LogWarning(warning);
            }
            
            if (results.HasErrors())
                throw new GoapException($"GoapSetConfig has errors: {config.Name}");
        }
        
        private SensorRunner CreateSensorRunner(IGoapSetConfig config)
        {
            return new SensorRunner(this.GetWorldSensors(config), this.GetTargetSensors(config));
        }
        
        private List<IActionBase> GetActions(IGoapSetConfig config)
        {
            var actions = new ClassResolver().Load<IActionBase, IActionConfig>(config.Actions);
            var injector = this.goapConfig.GoapInjector;
            
            actions.ForEach(x =>
            {
                injector.Inject(x);
                x.Created();
            });

            return actions;
        }
        
        private List<IGoalBase> GetGoals(IGoapSetConfig config)
        {
            var goals = new ClassResolver().Load<IGoalBase, IGoalConfig>(config.Goals);
            var injector = this.goapConfig.GoapInjector;
            
            goals.ForEach(x =>
            {
                injector.Inject(x);
            });

            return goals;
        }
        
        private List<IWorldSensor> GetWorldSensors(IGoapSetConfig config)
        {
            var worldSensors = new ClassResolver().Load<IWorldSensor, IWorldSensorConfig>(config.WorldSensors);
            var injector = this.goapConfig.GoapInjector;
            
            worldSensors.ForEach(x =>
            {
                injector.Inject(x);
                x.Created();
            });
            
            return worldSensors;
        }
        
        private List<ITargetSensor> GetTargetSensors(IGoapSetConfig config)
        {
            var targetSensors = new ClassResolver().Load<ITargetSensor, ITargetSensorConfig>(config.TargetSensors);
            var injector = this.goapConfig.GoapInjector;
            
            targetSensors.ForEach(x =>
            {
                injector.Inject(x);
                x.Created();
            });
            
            return targetSensors;
        }
    }
}