using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ServiceCore.DataAccess.SettingsEF
{
    public class ApplicationContext : BaseDbContext, IDbContext
    {
        private static readonly object FluentSettingsLock = new object();
        private static bool _isDapperMapInitialized;

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
            var isSeedNeeded = false;
            if(!isSeedNeeded)
                return;

            // открываем транзакцию, чтобы коммитить все изменения разом
            BeginTransaction();
            Flush();
            Commit();
        }
    }
}