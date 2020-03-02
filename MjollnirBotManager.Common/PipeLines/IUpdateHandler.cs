using System;
using Telegram.Bot.Types;

namespace MjollnirBotManager.Common.PipeLines
{
    public interface IUpdateHandler : IHandler<IUpdateHandler, Update>
    {

    }
}
