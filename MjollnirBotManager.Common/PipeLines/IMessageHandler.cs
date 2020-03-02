using System;
using MjollnirBotManager.Common.Security;
using Telegram.Bot.Types;

namespace MjollnirBotManager.Common.PipeLines
{
    public interface IMessageHandler : IHandler<IMessageHandler, Message>
    {
    }
}
