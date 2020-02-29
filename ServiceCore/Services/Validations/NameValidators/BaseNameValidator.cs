using System.Threading.Tasks;

namespace ServiceCore.Services.Validations.NameValidators
{
    /// <summary>
    ///     Базовая реализация, которая оборачивает вызов простой синхронной проверки в выполнение в отдельном таске
    /// </summary>
    public abstract class BaseNameValidator : INameValidator
    {
        /// <inheritdoc />
        public async Task<bool> ValidateAsync(string name)
        {
            return await Task.Run(() => ValidateSync(name));
        }

        internal abstract bool ValidateSync(string name);
    }
}