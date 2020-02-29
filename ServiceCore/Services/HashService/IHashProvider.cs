namespace ServiceCore.Services.HashService
{
    /// <summary>
    ///     Интерфейс конвертации строк в Hash
    /// </summary>
    public interface IHashProvider
    {
        /// <summary>
        ///     Получить hash текста
        /// </summary>
        /// <param name="textToHash">Текст, хэш которого нужно посчитать</param>
        /// <returns>Значение хэша для данного текста</returns>
        string GetTextHash(string textToHash);

        /// <summary>
        ///     Получить hash текста
        /// </summary>
        /// <param name="textToHash">Текст, хэш которого нужно посчитать</param>
        /// <param name="salt">Соль для использования при вычислении хэша</param>
        /// <returns>Значение хэша для данного текста</returns>
        string GetTextHash(string textToHash, string salt);
    }
}