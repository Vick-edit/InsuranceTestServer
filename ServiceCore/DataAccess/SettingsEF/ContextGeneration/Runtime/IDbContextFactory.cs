namespace ServiceCore.DataAccess.SettingsEF
{
    /// <summary>
    ///     Интерфейс абстрактной фабрики соединений с БД проекта
    /// </summary>
    public interface IDbContextFactory
    {
        /// <summary> Получить основное соединение с БД, в которой хранятся доменные сущности проекта </summary>
        IDbContext GetApplicationContext();
    }
}