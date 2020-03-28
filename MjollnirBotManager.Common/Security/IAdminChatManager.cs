using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MjollnirBotManager.Common.Dependency;
using Telegram.Bot.Types;

namespace MjollnirBotManager.Common.Security
{
    public interface IAdminChatManager : ISingleton
    {
        Task<IList<ChatId>> GetAllAdminChatsAsync();
        Task AddAdminChatAsync(long chatId);
        Task AddAdminChatAsync(ChatId chatId);
        Task RemoveAdminChatAsync(long chatId);
        Task RemoveAdminChatAsync(ChatId chatId);
    }
}
