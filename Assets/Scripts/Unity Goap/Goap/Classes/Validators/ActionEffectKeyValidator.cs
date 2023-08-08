using System.Linq;
using Goap.Configs.Interfaces;
using Goap.Interfaces;

namespace Goap.Classes.Validators
{
    public class ActionEffectKeyValidator : IValidator<IGoapSetConfig>
    {
        public void Validate(IGoapSetConfig config, ValidationResults results)
        {
            foreach (var configAction in config.Actions)
            {
                var missing = configAction.Effects.Where(x => x.WorldKey == null).ToArray();
                
                if (!missing.Any())
                    continue;
                
                results.AddError($"Action {configAction.Name} has effects without WorldKey");
            }
        }
    }
}