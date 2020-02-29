
using System;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ServiceCore.DataAccess.SettingsEF
{
    /// <summary>
    ///     Интерфейс для взаимодействия с БД
    /// </summary>
    public interface IDbContext : IDisposable
    {
        bool AutoCommit { get; set; }

        /// <summary> Привести состояние БД в соответствие с заданной схемой объектов </summary>
        void Migrate();

        /// <summary> Заполнить БД тестовыми данными, если БД пустая </summary>
        void Seed();

        /// <summary> Получить объект соединения с базой </summary>
        DbConnection GetConnection();

        /// <summary> Совойства для доступа к сущностям БД </summary>
        /// <typeparam name="T">Тип сущности БД</typeparam>
        /// <returns>Объект для взаимодействия с сущностью из БД</returns>
        DbSet<T> Set<T>() where T : class;

        /// <summary> Управление сущность с привязкой к контексту </summary>
        /// <typeparam name="T">Тип сущности</typeparam>
        /// <param name="entity">Сущность, контекст которой нужно получить</param>
        EntityEntry<T> Entry<T>(T entity) where T : class;


        /// <summary> В рамках существующей сущности <see cref="IDbContext"/> начинает транзакцию </summary>
        IDbContext BeginTransaction();

        /// <summary> Сохранить изменения в контексте транзакции, но не завершать транзакцию </summary>
        void Flush();

        /// <summary> Сохранить изменения в БД </summary>
        void Commit();


        /// <summary> Добавить или обновить в БД новые записи </summary>
        EntityEntry<T> CreateOrUpdate<T>(T entity) where T : class;

        /// <summary> Удалить сущность </summary>
        EntityEntry<T> Delete<T>(T entity) where T : class;
    }
}