using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MjollnirBotManager.Common.Dependency;
using Telegram.Bot.Types;

namespace MjollnirBotManager.Common.Security
{
    public interface IAdminChatManager : ISingleton
    {
        Task<IList<ChatId>> GetAllAdminChats();
        Task AddAdminChat(long chatid);
        Task AddAdminChat(ChatId chatid);
        Task RemoveAdminChat(long chatid);
        Task RemoveAdminChat(ChatId chatid);
    }
}
