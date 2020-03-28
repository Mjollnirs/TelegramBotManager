using System;
using System.Threading;
using System.Threading.Tasks;
using Castle.Core.Logging;
using MjollnirBotManager.Common.PipeLines;
using MjollnirBotManager.Common.Store;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MjollnirBotManager.PipeLines
{
    internal sealed class DefaultUpdateHandler : UpdateHandlerBase
    {
        private readonly IPipeLine<IMessageHandler, Message> _messagePipeLine;
        private readonly IChatManager _chatManager;

        public DefaultUpdateHandler(ILogger logger,
            IPipeLine<IMessageHandler, Message> messagePipeLine,
            IChatManager chatManager)
            : base(logger)
        {
            _messagePipeLine = messagePipeLine;
            _chatManager = chatManager;
            Order = int.MinValue;
        }

        protected async override Task ProcessHandler(UpdateType updateType, Update update, CancellationToken token)
        {
            Logger.InfoFormat("ProcessHandler update {0} {1} end.", update.Id, updateType);
            switch (updateType)
            {
                case UpdateType.Message:
                    _ = Task.Run(async () =>
                    {
                        _ = _chatManager.AddAsync(Update.Message.Chat);
                        await _messagePipeLine.InvokeAsync(Update.Message, token);
                    });
                    break;
                default:
                    break;
            }

            await Task.Yield();
        }
    }
}
