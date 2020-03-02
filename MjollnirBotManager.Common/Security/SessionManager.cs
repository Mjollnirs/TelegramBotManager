using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MjollnirBotManager.Common.Dependency;
using Telegram.Bot.Types;

namespace MjollnirBotManager.Common.Security
{
    internal sealed class SessionManager : ISessionManager, ISingleton
    {
        private readonly Dictionary<long, ISession> _sessions = new Dictionary<long, ISession>();

        public async Task<ISession> GetSessionAsync(ChatId chatId)
        {
            await Task.Yield();
            if (!_sessions.ContainsKey(chatId.Identifier))
                _sessions.Add(chatId.Identifier, new Session());

            return _sessions[chatId.Identifier];
        }
    }
}
