using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Core.Logging;
using LiteDB;
using MjollnirBotManager.Common.Store;
using Telegram.Bot.Types;

namespace MjollnirBotManager.Common.Security
{
    internal sealed class AdminChatManager : IAdminChatManager
    {
        private readonly ILogger _logger;
        private readonly IChatManager _chatManager;

        public AdminChatManager(ILogger logger, IChatManager chatManager)
        {
            _logger = logger;
            _chatManager = chatManager;
        }

        public async Task AddAdminChatAsync(ChatId chatId) =>
            await AddAdminChatAsync(chatId.Identifier);

        public async Task AddAdminChatAsync(long chatId)
        {
            await _chatManager.AddAsync(chatId, true);
        }

        public async Task RemoveAdminChatAsync(ChatId chatId) =>
            await RemoveAdminChatAsync(chatId.Identifier);

        public async Task RemoveAdminChatAsync(long chatId)
        {
            await ((ChatManager)_chatManager).UpdateAdminAsync(chatId, false);
        }

        public async Task<IList<ChatId>> GetAllAdminChatsAsync()
        {
            await Task.Yield();
            var all = await ((ChatManager)_chatManager).GetAllAdminsAsync();
            return all.Select(x => new ChatId(x)).ToArray();
        }
    }
}
