using System.Collections.Generic;
using System.Threading.Tasks;
using MjollnirBotManager.Common.Dependency;
using Telegram.Bot.Types;

namespace MjollnirBotManager.Common.Store
{
    public interface IChatManager : ISingleton
    {
        Task AddAsync(ChatId chat, bool isAdmin = false);
        Task AddAsync(long chat, bool isAdmin = false);
        Task<bool> ContainsAsync(ChatId chat);
        Task<bool> ContainsAsync(long chat);
        Task<IList<ChatId>> GetAllChatsAsync();
        Task RemoveAsync(ChatId chat);
        Task RemoveAsync(long chat);
    }
}