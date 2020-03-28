using System.Threading;
using System.Threading.Tasks;
using Castle.Core.Logging;
using Castle.MicroKernel;
using MjollnirBotManager.Common.Dependency;
using MjollnirBotManager.Common.Security;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MjollnirBotManager.Common.PipeLines
{
    public abstract class MessageHandlerBase : Handler<IMessageHandler, Message>, IMessageHandler, ISingleton
    {
        private readonly IAdminChatValidator _adminChatValidator;
        private readonly ISessionManager _sessionManager;
        private readonly IKernel _kernel;

        protected IBotManager BotManager => _kernel.Resolve<IBotManager>();

        protected Message Message { get; private set; }
        protected MessageType MessageType => Message.Type;
        protected bool IsAdmin => _adminChatValidator.IsAdminAsync(Message).Result;
        protected Chat Chat => Message.Chat;

        protected ISession Session => _sessionManager.GetSessionAsync(Chat).Result;

        public MessageHandlerBase(
            IKernel kernel,
            ILogger logger,
            IAdminChatValidator adminChatValidator,
            ISessionManager sessionManager) : base(logger)
        {
            _kernel = kernel;
            _adminChatValidator = adminChatValidator;
            _sessionManager = sessionManager;
        }

        protected async override Task ProcessHandler(Message s, CancellationToken token)
        {
            Message = s;
            await ProcessHandler(Chat, MessageType, IsAdmin, Message, Session, token);
        }

        protected abstract Task ProcessHandler(Chat chat, MessageType messageType, bool isAdmin, Message message, ISession session, CancellationToken token);
    }
}
