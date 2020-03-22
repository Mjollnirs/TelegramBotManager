using System;
using System.Threading;
using System.Threading.Tasks;
using Castle.Core.Logging;
using MjollnirBotManager.Common.PipeLines;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MjollnirBotManager.PipeLines
{
    internal sealed class DefaultUpdateHandler : UpdateHandlerBase
    {
        private readonly IPipeLine<IMessageHandler, Message> _messagePipeLine;

        public DefaultUpdateHandler(ILogger logger, IPipeLine<IMessageHandler, Message> messagePipeLine)
            : base(logger)
        {
            _messagePipeLine = messagePipeLine;
            Order = int.MinValue;
        }

        protected async override Task ProcessHandler(UpdateType updateType, Update update, CancellationToken token)
        {
            Logger.InfoFormat("ProcessHandler update {0} {1} end.", update.Id, updateType);
            switch (updateType)
            {
                case UpdateType.Message:
                    await _messagePipeLine.InvokeAsync(Update.Message, token);
                    break;
                default:
                    break;
            }
        }
    }
}
