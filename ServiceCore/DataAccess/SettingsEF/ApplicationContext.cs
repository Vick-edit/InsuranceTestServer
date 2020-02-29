using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ServiceCore.DataAccess.Mapping;
using ServiceCore.Domain.Models;

namespace ServiceCore.DataAccess.SettingsEF
{
    public class ApplicationContext : BaseDbContext, IDbContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _dtConnectionString; //Строка соединения для использования с миграцией


        /// <inheritdoc />
        public ApplicationContext(IConfiguration configuration)
        {
            _configuration = configuration;
            AutoCommit = true;
        }

        /// <summary> Конструктор для использования с миграцией </summary>
        internal ApplicationContext(string dtConnectionString)
        {
            _dtConnectionString = dtConnectionString;
            AutoCommit = true;
        }


        /// <inheritdoc />
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = _configuration?.GetConnectionString("SqliteConnection") ?? _dtConnectionString;
                optionsBuilder.UseSqlite(connectionString);
            }
        }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ProductMap.Register(modelBuilder);
        }


        /// <inheritdoc />
        public override void Migrate()
        {
            try
            {
                base.Database.Migrate();
            }
            catch(Exception basException)
            {
                var msg = "Дело в том, что SQLite почти не умеет в миграции, поэтому, чтобы БД нормально отмигрировала она должна быть пустой";
                throw new DbUpdateException(msg, basException);
            }
        }

        /// <inheritdoc />
        public override void Seed()
        {
            // проверять все таблицы, что они пустые муторно, если таблица продуктов пустая, значит, скорее всего нужно выполнить внедрение данных
            var isSeedNeeded = !Set<Product>().Any();
            if (!isSeedNeeded)
                return;

            // открываем транзакцию, чтобы коммитить все изменения разом
            BeginTransaction();
            Set<Product>().AddRange(SeedData.TestProducts);
            Commit();
        }
    }
}