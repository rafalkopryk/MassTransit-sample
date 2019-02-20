using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Messaging.Common.Commands;
using static System.Console;

namespace Messaging.Console.Handlers
{
    public class SubmitMessageHandler : IConsumer<ISubmitMessage>
    {
        public async Task Consume(ConsumeContext<ISubmitMessage> context)
        {
            var message = context.Message;
            await Out.WriteLineAsync($"{string.Join(' ',Enumerable.Repeat('-', 20))}");
            await Out.WriteLineAsync($"From {message.Who}");
            await Out.WriteLineAsync($"Date {message.Timestamp:yyyy-M-d dddd hh:ss}");
            await Out.WriteLineAsync($"Date {message.Content}");
            await Out.WriteLineAsync($"{string.Join(' ',Enumerable.Repeat('-', 20))}");
        }
    }
}
