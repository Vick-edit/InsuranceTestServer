using System;
using Microsoft.Extensions.DependencyInjection;
using ServiceCore.DataAccess.SettingsEF;
using SimpleInjector;

namespace ServiceCore.Settings
{
    /// <summary>
    ///     Класс с DI настройками для библиотеки <see cref="nameof(BuildStoreCore)"/> по-умолчанию, которые используются внутри самой библиотеки
    /// </summary>
    public static class DISettings
    {
        private static readonly object ContainerLock = new object();
        private static IServiceProvider Provider { get; set; }


        /// <summary> Зарегистрировать типы из библиотеки </summary>
        public static void RegisterDependencies(Container container)
        {
            container.Register<IDbContextFactory, DbContextFactory>();

            lock (ContainerLock)
            {
                if (Provider != null)
                    throw new Exception("IoC уже был проинициализирован для этой библиотеки, повторная инициализация может привести к непредвиденному поведению");

                Provider = container;
            }
        }
    }
}