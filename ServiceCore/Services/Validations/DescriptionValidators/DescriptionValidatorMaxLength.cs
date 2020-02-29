using ServiceCore.Domain.Models;
using ServiceCore.Settings;

namespace ServiceCore.Services.Validations.DescriptionValidators
{
    /// <summary>
    ///     Проверка допустимой длины имени продукта <see cref="Product.Name"/>
    /// </summary>
    public class DescriptionValidatorMaxLength : BaseDescriptionValidator, IDescriptionValidator
    {
        internal override bool ValidateSync(string description)
        {
            if (description == null)
                return true;

            return description.Length < AppCoreConstants.MAX_PRODUCT_DESCRIPTION_LENGTH;
        }
    }
}