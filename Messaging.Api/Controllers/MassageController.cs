using System;
using System.Threading.Tasks;
using MassTransit;
using Messaging.Common;
using Messaging.Common.Commands;
using Messaging.Common.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Messaging.Api.Controllers
{
    [Route("api/[controller]")]
    public class MassageController : Controller
    {
        private readonly Task<ISendEndpoint> _messageExchange;

        public MassageController(IOptions<RabbitMqConfig> config, ISendEndpointProvider busControl)
        {
            _messageExchange = busControl.GetSendEndpoint(config.Value.MessageQueueFullPath);
        }

        [HttpPost]
        public async Task Post([FromBody] SubmitMessageDto message)
        {
            var exchange = await _messageExchange;
            await exchange.Send<ISubmitMessage>(new
            {
                Timestamp = DateTime.UtcNow,
                message.Who,
                message.Content
            });
        }
    }
}
