using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Castle.MicroKernel;
using MjollnirBotManager.Common.Configuration;
using MjollnirBotManager.Common.PipeLines;
using MjollnirBotManager.Common.Security;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

namespace MjollnirBotManager.PipeLines
{
    internal sealed class ForwardMessageHandler : MessageHandlerBase
    {
        private readonly IAdminUserManager _adminUserManager;
        private readonly IBotConfiguration _config;

        private ChatId preChatId;

        public ForwardMessageHandler(
            IKernel kernel,
            IBotConfiguration config,
            IAdminUserManager adminUserManager,
            IAdminUserValidator userValidator,
            ISessionManager sessionManager)
            : base(kernel, userValidator, sessionManager)
        {
            _adminUserManager = adminUserManager;
            _config = config;
        }

        protected async override Task ProcessHandler(Chat chat, MessageType messageType, bool isAdmin, Message message, ISession session, CancellationToken token)
        {
            if (isAdmin && chat.Type == ChatType.Private)
            {
                ChatId id = preChatId;
                if (message.ReplyToMessage != null)
                    id = message.ReplyToMessage.Chat;

                if (id == null)
                    id = new ChatId(_config.DefaultChat);

                switch (messageType)
                {
                    case MessageType.Text:
                        await BotManager.Telegram.SendTextMessageAsync(id,
                            message.Text,
                            cancellationToken: token);
                        break;
                    case MessageType.Sticker:
                        await BotManager.Telegram.SendStickerAsync(id,
                            new InputOnlineFile(message.Sticker.FileId),
                            cancellationToken: token);
                        break;
                    case MessageType.Photo:
                        await BotManager.Telegram.SendPhotoAsync(id,
                            new InputOnlineFile(message.Photo.OrderByDescending(x => x.FileSize).First().FileId),
                            cancellationToken: token);
                        break;
                    default:
                        break;
                }
            }
            else if (!isAdmin)
            {
                preChatId = chat;
                await ForwardToAdmin(message);
            }
        }

        private async Task ForwardToAdmin(Message message)
        {
            foreach (var item in await _adminUserManager.GetAllAdminUserChatId())
            {
                await BotManager.Telegram.ForwardMessageAsync(item, message.Chat, message.MessageId);
            }
        }
    }
}
