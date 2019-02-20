using System;
using MassTransit;
using MassTransit.RabbitMqTransport;

namespace Messaging.Common
{
    public class Configurator
    {
        public static IBusControl ConfigureBus((string Uri, string UserName, string Password) config,
            Action<IRabbitMqBusFactoryConfigurator, IRabbitMqHost> registrationAction = null)
        {
            return Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var (uri, userName, password) = config;
                var host = cfg.Host(new Uri(uri), hostBuilder =>
                {
                    hostBuilder.Username(userName);
                    hostBuilder.Password(password);
                });

                registrationAction?.Invoke(cfg, host);
            });
        }
    }
}
