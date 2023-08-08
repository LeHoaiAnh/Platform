using System.Linq;
using Goap.Configs.Interfaces;
using Goap.Interfaces;

namespace Goap.Classes.Validators
{
    public class ActionTargetValidator : IValidator<IGoapSetConfig>
    {
        public void Validate(IGoapSetConfig config, ValidationResults results)
        {
            var missing = config.Actions.Where(x => x.Target == null).ToArray();
            
            if (!missing.Any())
                return;
            
            results.AddWarning($"Actions without Target: {string.Join(", ", missing.Select(x => x.Name))}");
        }
    }
}