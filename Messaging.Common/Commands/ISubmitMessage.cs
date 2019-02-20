using System;
using System.Collections.Generic;
using System.Text;

namespace Messaging.Common.Commands
{
    public interface ISubmitMessage
    {
        DateTime Timestamp { get; }
        string Who { get; }
        string Content { get; }
    }
}
