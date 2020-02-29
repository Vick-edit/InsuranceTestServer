using System;

namespace ServiceCore.Extensions
{
    /// <summary>
    ///     Расширение класса <see cref="IServiceProvider"/>
    /// </summary>
    internal static class IServiceProviderExtension
    {
        /// <summary> Получить объект типа <see cref="T"/> через DI </summary>
        /// <typeparam name="T">Тип извлекаемого объекта</typeparam>
        /// <param name="serviceProvider"><see cref="IServiceProvider"/> которым мы пользуемся для построение объекта</param>
        /// <returns>Объект искомого типа или null</returns>
        public static T Resolve<T>(this IServiceProvider serviceProvider) where T : class
        {
            return serviceProvider.GetService(typeof(T)) as T;
        }
    }
}