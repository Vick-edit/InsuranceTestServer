using ServiceCore.Domain.Models;

namespace ServiceCore.Services.Validations.NameValidators
{
    /// <summary>
    ///     Валидатор того, что в качестве имени <see cref="Product.Name"/> не используется null или пустая строка
    /// </summary>
    public class NameValidatorNotEmpty : BaseNameValidator, INameValidator
    {
        internal override bool ValidateSync(string name)
        {
            return !string.IsNullOrEmpty(name);
        }
    }
}