using System;
using System.Threading;
using System.Threading.Tasks;
using MjollnirBotManager.Common.Dependency;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MjollnirBotManager.Common.PipeLines
{
    public abstract class UpdateHandlerBase : Handler<IUpdateHandler, Update>, IUpdateHandler, ISingleton
    {
        protected Update Update { get; private set; }
        protected UpdateType UpdateType => Update.Type;

        protected override async Task ProcessHandler(Update s, CancellationToken token)
        {
            Update = s;
            await ProcessHandler(UpdateType, Update, token);
        }

        protected abstract Task ProcessHandler(UpdateType updateType, Update update, CancellationToken token);
    }
}
