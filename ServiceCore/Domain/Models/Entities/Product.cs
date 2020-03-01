using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using ServiceCore.Settings;

namespace ServiceCore.Domain.Models
{
    /// <summary>
    ///     Основная сущность системы, хронит в себе описание модели, настройки валидации и сериализации
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Product
    {
        [Required(ErrorMessage = "Не указан Id продукта")]
        [Range(1, long.MaxValue, ErrorMessage = "Id продукта может принимать значение только от 1 до 9,223372036854776E+18")]
        [JsonProperty("id", Order = 1)]
        public virtual long Id { get; set; }

        [Required(ErrorMessage = "Не указано название продукта")]
        [StringLength(AppCoreConstants.MAX_PRODUCT_NAME_LENGTH, ErrorMessage = "Превышена допустимая длина имени продукта")]
        [JsonProperty("name", Order = 2)]
        public string Name { get; set; }

        [StringLength(AppCoreConstants.MAX_PRODUCT_DESCRIPTION_LENGTH, ErrorMessage = "Превышена допустимая длина описания продукта")]
        [JsonProperty("description", Order = 3)]
        public string Description { get; set; }
    }
}