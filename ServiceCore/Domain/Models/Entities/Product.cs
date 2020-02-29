using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

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
        [StringLength(200)]
        [JsonProperty("name", Order = 2)]
        public string Name { get; set; }

        [StringLength(500)]
        [JsonProperty("description", Order = 3)]
        public string Description { get; set; }
    }
}