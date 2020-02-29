using System.Threading.Tasks;

namespace ServiceCore.Services.Validations.DescriptionValidators
{
    /// <summary>
    ///     Базовая реализация, которая оборачивает вызов простой синхронной проверки в выполнение в отдельном таске
    /// </summary>
    public abstract class BaseDescriptionValidator : IDescriptionValidator
    {
        /// <inheritdoc />
        public async Task<bool> ValidateAsync(string description)
        {
            return await Task.Run(() => ValidateSync(description));
        }

        internal abstract bool ValidateSync(string description);
    }
}