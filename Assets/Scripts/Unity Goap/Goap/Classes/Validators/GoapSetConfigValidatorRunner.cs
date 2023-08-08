﻿using System.Collections.Generic;
using Goap.Configs.Interfaces;
using Goap.Interfaces;

namespace Goap.Classes.Validators
{
    public class GoapSetConfigValidatorRunner : IGoapSetConfigValidatorRunner
    {
        private readonly List<IValidator<IGoapSetConfig>> validators = new ()
        {
            new WorldKeySensorsValidator(),
            new TargetKeySensorsValidator(),
            new ActionClassTypeValidator(),
            new GoalClassTypeValidator(),
            new TargetSensorClassTypeValidator(),
            new WorldSensorClassTypeValidator(),
            new ActionEffectsValidator(),
            new GoalConditionsValidator(),
            new ActionTargetValidator(),
            new ActionEffectKeyValidator(),
            new ActionConditionKeyValidator(),
            new GoalConditionsValidator(),
            new GoalConditionKeyValidator(),
            new WorldSensorKeyValidator(),
            new TargetSensorKeyValidator()
        };
        
        public ValidationResults Validate(IGoapSetConfig config)
        {
            var results = new ValidationResults(config.Name);
            
            foreach (var validator in this.validators)
            {
                validator.Validate(config, results);
            }

            return results;
        }
    }
}