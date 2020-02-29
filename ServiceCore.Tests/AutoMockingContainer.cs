using SimpleInjector;
using System;
using Moq;

namespace ServiceCore.Tests
{
    public class AutoMockingContainer
    {
        public static Container GetNew()
        {
            var container = new Container();
            container.ResolveUnregisteredType += ResolveUnregisteredType;
            return container;
        }


        private static void ResolveUnregisteredType(object sender, UnregisteredTypeEventArgs eventArgs)
        {
            var container = (Container)sender;

            //Регистрируем создание объекта типа Mock напрямую
            if (typeof(Mock).IsAssignableFrom(eventArgs.UnregisteredServiceType))
            {
                eventArgs.Register(
                    Lifestyle.Singleton.CreateRegistration(
                        eventArgs.UnregisteredServiceType,
                        () => Activator.CreateInstance(eventArgs.UnregisteredServiceType),
                        container
                    )
                );
            }

            //Регистрируем создание инстансов любых интерфейсов через создание соответствующего мока
            if (eventArgs.UnregisteredServiceType.IsInterface)
            {
                Type mockType = typeof(Mock<>).MakeGenericType(eventArgs.UnregisteredServiceType);
                eventArgs.Register(
                    Lifestyle.Singleton.CreateRegistration(
                        eventArgs.UnregisteredServiceType,
                        () => ((Mock)container.GetInstance(mockType)).Object,
                        container
                    )
                );
            }
        }
    }
}
