using Goap.Classes.Validators;
using Goap.Configs.Interfaces;

namespace Goap.Interfaces
{
    public interface IGoapSetConfigValidatorRunner
    {
        ValidationResults Validate(IGoapSetConfig config);
    }
}