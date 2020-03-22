using System;
using System.Threading.Tasks;
using MjollnirBotManager.Common.Dependency;
using Telegram.Bot.Types;

namespace MjollnirBotManager.Common.Security
{
    public interface IAdminChatValidator : ISingleton
    {
        Task<bool> IsAdmin(Message message);
        Task<bool> IsAdmin(ChatId message);
        Task<bool> IsAdmin(User message);
        Task<bool> IsAdmin(long id);
    }
}
