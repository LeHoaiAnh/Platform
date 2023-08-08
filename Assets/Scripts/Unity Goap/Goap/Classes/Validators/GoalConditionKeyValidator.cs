using System.Linq;
using Goap.Configs.Interfaces;
using Goap.Interfaces;

namespace Goap.Classes.Validators
{
    public class GoalConditionKeyValidator : IValidator<IGoapSetConfig>
    {
        public void Validate(IGoapSetConfig config, ValidationResults results)
        {
            foreach (var configGoal in config.Goals)
            {
                var missing = configGoal.Conditions.Where(x => x.WorldKey == null).ToArray();
                
                if (!missing.Any())
                    continue;
                
                results.AddError($"Goal {configGoal.Name} has conditions without WorldKey");
            }
        }
    }
}