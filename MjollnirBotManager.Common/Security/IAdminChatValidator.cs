using System;
using System.Threading.Tasks;
using MjollnirBotManager.Common.Dependency;
using Telegram.Bot.Types;

namespace MjollnirBotManager.Common.Security
{
    public interface IAdminChatValidator : ISingleton
    {
        Task<bool> IsAdminAsync(Message message);
        Task<bool> IsAdminAsync(ChatId message);
        Task<bool> IsAdminAsync(User message);
        Task<bool> IsAdminAsync(long id);
    }
}
