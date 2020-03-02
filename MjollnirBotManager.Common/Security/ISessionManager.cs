using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace MjollnirBotManager.Common.Security
{
    public interface ISessionManager
    {
        Task<ISession> GetSessionAsync(ChatId chatId);
    }
}
