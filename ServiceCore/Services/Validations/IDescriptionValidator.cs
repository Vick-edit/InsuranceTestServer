using System.Threading.Tasks;
using ServiceCore.Domain.Models;

namespace ServiceCore.Services.Validations
{
    /// <summary>
    ///     Интерфейс валидации данных в поле <see cref="Product.Description"/>
    /// </summary>
    public interface IDescriptionValidator
    {
        /// <summary> Проверить может ли быть использован указанный текст в качестве <see cref="Product.Description"/> </summary>
        Task<bool> ValidateAsync(string description);
    }
}