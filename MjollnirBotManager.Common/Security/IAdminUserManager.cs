using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MjollnirBotManager.Common.Dependency;
using Telegram.Bot.Types;

namespace MjollnirBotManager.Common.Security
{
    public interface IAdminUserManager : ISingleton
    {
        Task<IList<string>> GetAllAdminUser();
        Task<IList<ChatId>> GetAllAdminUserChatId();
        Task AddAdminUser(string username);
        Task RemoveAdminUser(string username);
    }
}
