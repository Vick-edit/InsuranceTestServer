using ServiceCore.Domain.Models;

namespace ServiceCore.Settings
{
    /// <summary>
    ///     Набор констант ядра приложения
    /// </summary>
    public static class AppCoreConstants
    {
        /// <summary> Максимальная длина имени продукта <see cref="Product.Name"/> </summary>
        public const int MAX_PRODUCT_NAME_LENGTH = 200;

        /// <summary> Минимальная длина имени продукта <see cref="Product.Name"/> </summary>
        public const int MIN_PRODUCT_NAME_LENGTH = 4;

        /// <summary> Максимальная длина описания продукта <see cref="Product.Description"/> </summary>
        public const int MAX_PRODUCT_DESCRIPTION_LENGTH = 500;
    }
}