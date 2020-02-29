using Microsoft.Extensions.Configuration;

namespace ServiceCore.DataAccess.SettingsEF
{
    /// <summary>
    ///     Основная реализация фабрики соединений с БД на базе EF
    /// </summary>
    public class DbContextFactory : IDbContextFactory
    {
        private readonly IConfiguration _configuration;


        public DbContextFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        /// <inheritdoc />
        public IDbContext GetApplicationContext()
        {
            return new ApplicationContext(_configuration);
        }
    }
}