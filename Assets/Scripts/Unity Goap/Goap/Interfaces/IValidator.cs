using Goap.Classes.Validators;

namespace Goap.Interfaces
{
    public interface IValidator<T>
    {
        void Validate(T config, ValidationResults results);
    }
}