using System;
using System.Threading;
using System.Threading.Tasks;
using Castle.Core.Logging;
using Castle.MicroKernel;
using MjollnirBotManager.Common.Command;
using MjollnirBotManager.Common.PipeLines;
using MjollnirBotManager.Common.Security;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MjollnirBotManager.PipeLines
{
    internal sealed class CommandMessageHandler : MessageHandlerBase
    {
        private readonly ICommandManager _commandManager;

        public ICommand PrevCommand { get; private set; }

        public CommandMessageHandler(
            IKernel kernel,
            ILogger logger,
            IAdminChatValidator adminChatValidator,
            ICommandManager commandManager,
            ISessionManager sessionManager)
            : base(kernel, logger, adminChatValidator, sessionManager)
        {
            _commandManager = commandManager;
            Order = int.MinValue;
        }

        protected async override Task ProcessHandler(Chat chat, MessageType messageType, bool isAdmin, Message message, ISession session, CancellationToken token)
        {
            Logger.InfoFormat("Receiver message {0} in chat {1}, Entity Length: {2}, Admin: {3}, IsCommand: {4}",
                messageType,
                chat.Type,
                message.Entities?.Length,
                isAdmin,
                await _commandManager.IsCommandAsync(message));

            if (await _commandManager.IsCommandAsync(message))
            {
                if (PrevCommand != null)
                    await PrevCommand?.Reset();
                PrevCommand = await _commandManager.ExecuteAsync(message, token);
                await BreakAsync();
            }
        }
    }
}
