using System;
using System.Threading.Tasks;
using MjollnirBotManager.Common.Dependency;
using Telegram.Bot.Types;

namespace MjollnirBotManager.Common.Security
{
    public interface IAdminUserValidator : ISingleton
    {
        Task<bool> IsAdmin(Message message);
        Task<bool> IsAdmin(ChatId message);
        Task<bool> IsAdmin(User message);
        Task<bool> IsAdmin(string username);
    }
}
