using System;
using System.Threading;
using System.Threading.Tasks;
using Castle.Core.Logging;
using Castle.MicroKernel;
using MjollnirBotManager.Common.Security;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MjollnirBotManager.Common.Command
{
    public abstract class CommandBase : ICommand
    {
        private readonly ILogger _logger;
        private readonly IAdminChatValidator _adminChatValidator;
        private readonly ISessionManager _sessionManager;

        protected ICommandManager CommandManager { get; }
        protected IBotManager BotManager { get; }

        protected Message Message { get; private set; }
        protected MessageType MessageType => Message.Type;
        protected bool IsAdmin => _adminChatValidator.IsAdmin(Message).Result;
        protected Chat Chat => Message.Chat;

        protected ISession Session => _sessionManager.GetSessionAsync(Chat).Result;

        protected CommandBase(IKernel kernel)
        {
            CommandManager = kernel.Resolve<ICommandManager>();
            BotManager = kernel.Resolve<IBotManager>();
            _logger = kernel.Resolve<ILogger>().CreateChildLogger(nameof(CommandBase));
            _adminChatValidator = kernel.Resolve<IAdminChatValidator>();
            _sessionManager = kernel.Resolve<ISessionManager>();
        }

        public async Task Handle(Message message, CancellationToken token)
        {
            Message = message;
            _logger.DebugFormat("Handle Command {0}", GetType().Name);
            await Process(token);
        }

        public async Task Reset()
        {
            await ResetHandle();
        }

        protected abstract Task Process(CancellationToken token);
        protected async virtual Task ResetHandle()
        {
            await Task.Yield();
        }
    }
}
