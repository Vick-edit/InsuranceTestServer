using ServiceCore.Settings;
using ServiceCore.Domain.Models;

namespace ServiceCore.Services.Validations.NameValidators
{
    /// <summary>
    ///     Проверка допустимой длины имени продукта <see cref="Product.Name"/>
    /// </summary>
    public class NameValidatorMinLength : BaseNameValidator, INameValidator
    {
        internal override bool ValidateSync(string name)
        {
            if (string.IsNullOrEmpty(name))
                return false;

            return name.Length >= AppCoreConstants.MIN_PRODUCT_NAME_LENGTH;
        }
    }
}