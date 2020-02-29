using System;
using Microsoft.Extensions.DependencyInjection;
using ServiceCore.DataAccess.SettingsEF;
using ServiceCore.Services.HashService;
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
            container.Register<IHashProvider, HashByPbkdf2Sha256>();

            if (Provider != null)
                throw new Exception("IoC уже был проинициализирован для этой библиотеки, повторная инициализация может привести к непредвиденному поведению");

            lock (ContainerLock)
            {
                if (Provider != null)
                    throw new Exception("IoC уже был проинициализирован для этой библиотеки, повторная инициализация может привести к непредвиденному поведению");

                Provider = container;
            }
        }


        /// <summary> Получить объект <see cref="Lazy{T}"/> с инициализацией из <see cref="IServiceProvider"/> который был зарегистрирован в <see cref="RegisterDependencies"/> </summary>
        internal static Lazy<TInstance> GetLazyInstance<TInstance>()
        {
            return new Lazy<TInstance>(Provider.GetService<TInstance>);
        }


        private static bool IsGenericNotHandled(PredicateContext context)
        {
            return !context.Handled && context.ImplementationType.Namespace.StartsWith(nameof(ServiceCore));
        }
    }
}