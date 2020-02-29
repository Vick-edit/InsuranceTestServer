using ServiceCore.Settings;
using ServiceCore.Domain.Models;

namespace ServiceCore.Services.Validations.NameValidators
{
    /// <summary>
    ///     Проверка допустимой длины имени продукта <see cref="Product.Name"/>
    /// </summary>
    public class NameValidatorMaxLength : BaseNameValidator, INameValidator
    {
        internal override bool ValidateSync(string name)
        {
            if (name == null)
                return true;

            return name.Length < AppCoreConstants.MAX_PRODUCT_NAME_LENGTH;
        }
    }
}