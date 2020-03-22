using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Core.Logging;
using Telegram.Bot.Types;

namespace MjollnirBotManager.Common.Security
{
    internal sealed class AdminChatManager : IAdminChatManager
    {
        private readonly IList<long> _chats = new List<long>();
        private readonly ILogger _logger;

        public AdminChatManager(ILogger logger)
        {
            _logger = logger;
        }

        public async Task AddAdminChat(long chatid)
        {
            if (!_chats.Contains(chatid))
            {
                _logger.InfoFormat("Add Admin Chat: {0}", chatid);
                _chats.Add(chatid);
            }
            await Task.Yield();
        }

        public async Task AddAdminChat(ChatId chatid)
        {
            await AddAdminChat(chatid.Identifier);
        }

        public async Task<IList<ChatId>> GetAllAdminChats()
        {
            await Task.Yield();
            return _chats.Select(x => new ChatId(x)).ToArray();
        }

        public async Task RemoveAdminChat(long chatid)
        {
            if (_chats.Contains(chatid))
            {
                _logger.InfoFormat("Remove Admin Chat: {0}", chatid);
                _chats.Remove(chatid);
            }
            await Task.Yield();
        }

        public async Task RemoveAdminChat(ChatId chatid)
        {
            await RemoveAdminChat(chatid.Identifier);
        }
    }
}
