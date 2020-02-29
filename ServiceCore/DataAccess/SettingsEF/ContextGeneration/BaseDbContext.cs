using System;
using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace ServiceCore.DataAccess.SettingsEF
{
    /// <summary>
    ///     Базовая реализация <see cref="IDbContext"/> на основе EF core <see cref="DbContext"/>
    /// </summary>
    public abstract class BaseDbContext : DbContext, IDbContext
    {
        public bool AutoCommit { get; set; }

        private IDbContextTransaction _activeTransaction;

        /// <inheritdoc />
        public IDbContext BeginTransaction()
        {
            if (_activeTransaction != null || base.Database.CurrentTransaction != null)
                throw new Exception("В рамках данного подключения к бд уже была создана транзакция");

            _activeTransaction = base.Database.BeginTransaction();
            return this;
        }

        /// <inheritdoc />
        public void Flush()
        {
            if (_activeTransaction == null)
                throw new Exception("Невозможно флашить изменения, в данном контексте не была создана транзакция");

            base.SaveChanges(true);
        }

        /// <inheritdoc />
        public void Commit()
        {
            base.SaveChanges(true);
            _activeTransaction?.Commit();
        }

        /// <inheritdoc />
        public virtual void Migrate()
        {
            base.Database.Migrate();
        }

        /// <inheritdoc />
        public virtual void Seed()
        {
            // поумолчанию никаких тестовых данных в БД загружаться не должно
            return;
        }

        /// <inheritdoc />
        public DbConnection GetConnection()
        {
            return base.Database.GetDbConnection();
        }

        /// <inheritdoc />
        public EntityEntry<T> CreateOrUpdate<T>(T entity) where T : class
        {
            return base.Update(entity);
        }

        /// <inheritdoc />
        public EntityEntry<T> Delete<T>(T entity) where T : class
        {
            return base.Remove(entity);
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            _activeTransaction?.Dispose();
            base.Dispose();
        }

        /// <inheritdoc />
        public override ValueTask DisposeAsync()
        {
            _activeTransaction?.Dispose();
            return base.DisposeAsync();
        }
    }
}