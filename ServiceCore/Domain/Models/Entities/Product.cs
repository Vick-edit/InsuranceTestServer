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
        [Required]
        [Range(1, long.MaxValue)]
        [JsonProperty("id", Order = 1)]
        public long Id { get; set; }

        [Required]
        [StringLength(AppCoreConstants.MAX_PRODUCT_NAME_LENGTH)]
        [JsonProperty("name", Order = 2)]
        public string Name { get; set; }

        [StringLength(AppCoreConstants.MAX_PRODUCT_DESCRIPTION_LENGTH)]
        [JsonProperty("description", Order = 3)]
        public string Description { get; set; }
    }
}