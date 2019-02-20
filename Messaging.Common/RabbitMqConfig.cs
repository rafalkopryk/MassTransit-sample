using System;
using System.IO;

namespace Messaging.Common
{
    public class RabbitMqConfig
    {
        public string Uri { get; set; }
        public string MessageQueue { get; set; }

        public Uri MessageQueueFullPath => new Uri(new Uri(Uri), MessageQueue);
    }

    public class RabbitMqAuthConfig : RabbitMqConfig
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
