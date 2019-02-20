using System;
using System.Threading.Tasks;
using GreenPipes;
using MassTransit;
using Messaging.Common;
using Messaging.Console.Handlers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Messaging.Console
{
    class Program
    {
        static async Task Main(string[] args) =>
            await new HostBuilder()
                .ConfigureHostConfiguration(config => config.AddEnvironmentVariables())
                .ConfigureAppConfiguration((context, builder) => ConfigureApp(context, builder, args))
                .ConfigureServices(ConfigureServices)
                .RunConsoleAsync();

        private static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            var configuration = context.Configuration;

            //Buss

            var rabbitMqUri = configuration.GetSection($"{nameof(RabbitMqConfig)}:{nameof(RabbitMqConfig.Uri)}")
                .Value;
            var userName = configuration.GetSection($"{nameof(RabbitMqConfig)}:{nameof(RabbitMqAuthConfig.UserName)}")
                .Value;
            var password = configuration.GetSection($"{nameof(RabbitMqConfig)}:{nameof(RabbitMqAuthConfig.Password)}")
                .Value;
            var config = (rabbitMqUri, userName, password);

            services.AddMassTransit(configurator =>
            {
                configurator.AddBus(provider => Configurator.ConfigureBus(config, (factoryConfigurator, host) =>
                {
                    var submitQueue = configuration
                        .GetSection($"{nameof(RabbitMqConfig)}:{nameof(RabbitMqConfig.MessageQueue)}").Value;
                    factoryConfigurator.ReceiveEndpoint(host, submitQueue, e =>
                    {
                        e.UseRetry(Retry.Except<ArgumentException>().Intervals(400));
                        e.UseRateLimit(100, TimeSpan.FromSeconds(1));
                        e.Consumer<SubmitMessageHandler>();
                    });
                }));
            });

            services.AddHostedService<MassTransitHostedService>();
        }

        private static void ConfigureApp(HostBuilderContext context, IConfigurationBuilder builder, string[] args)
        {
            var environmentName = context.HostingEnvironment.EnvironmentName;
            builder.AddJsonFile("appsettings.json", optional: true);
            builder.AddJsonFile($"appsettings.{environmentName}.json", optional: true);
            builder.AddCommandLine(args);
        }
    }
}
