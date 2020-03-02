using System;
using Castle.MicroKernel;
using MjollnirBotManager.Common.Dependency;
using Telegram.Bot.Types;

namespace MjollnirBotManager.Common.PipeLines
{
    internal sealed class MessagePipeLine : PipeLine<IMessageHandler, Message> , ISingleton
    {
        public MessagePipeLine(IKernel kernel)
            : base(kernel)
        {
        }
    }
}
