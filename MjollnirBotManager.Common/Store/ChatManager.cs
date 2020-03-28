using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Castle.Core.Logging;
using LiteDB;
using Telegram.Bot;
using Telegram.Bot.Types;
using System.Linq.Expressions;

namespace MjollnirBotManager.Common.Store
{
    internal sealed class ChatManager : IChatManager
    {
        private readonly ILogger _logger;
        private readonly ILiteDatabase _database;
        private readonly ILiteCollection<Chat> _chats;

        public ChatManager(ILogger logger, ILiteDatabase database)
        {
            _logger = logger;
            _database = database;
            _chats = _database.GetCollection<Chat>();
        }

        public async Task AddAsync(ChatId chat, bool isAdmin = false) =>
            await AddAsync(chat.Identifier, isAdmin);

        public async Task AddAsync(long chat, bool isAdmin = false)
        {
            if (!await ContainsAsync(chat))
            {
                _chats.Insert(new Chat(chat, isAdmin));
                _logger.DebugFormat("Store Chat {0}, IsAdmin: {1}", chat, isAdmin);
            }
        }

        public async Task RemoveAsync(ChatId chat) =>
            await RemoveAsync(chat.Identifier);

        public async Task RemoveAsync(long chat)
        {
            if (await ContainsAsync(chat))
            {
                _chats.DeleteMany(x => x.Identifier == chat);
                _logger.DebugFormat("Remove Chat {0}", chat);
            }
        }

        public async Task<bool> ContainsAsync(ChatId chat) =>
            await ContainsAsync(chat.Identifier);

        public async Task<bool> ContainsAsync(long chat)
        {
            await Task.Yield();
            return _chats.Find(x => x.Identifier == chat).Any();
        }

        public async Task<IList<ChatId>> GetAllChatsAsync()
        {
            await Task.Yield();
            return _chats.FindAll().Select(x => new ChatId(x.Identifier)).ToArray();
        }

        public async Task UpdateAdminAsync(ChatId chat, bool isAdmin) =>
            await UpdateAdminAsync(chat.Identifier, isAdmin);

        public async Task UpdateAdminAsync(long chat, bool isAdmin)
        {
            if (await ContainsAsync(chat))
            {
                var dto = _chats.FindOne(x => x.Identifier == chat);
                dto.IsAdmin = isAdmin;
                _chats.Update(dto);

                _logger.DebugFormat("Update Chat {0}, IsAdmin: {1}", chat, isAdmin);
            }
        }

        public async Task<IList<ChatId>> GetAllAdminsAsync()
        {
            await Task.Yield();
            return _chats.Find(x => x.IsAdmin).Select(x => new ChatId(x.Identifier)).ToArray();
        }
    }
}
