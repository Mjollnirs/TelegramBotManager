using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Core.Logging;
using Telegram.Bot.Types;

namespace MjollnirBotManager.Common.Security
{
    internal sealed class AdminUserManager : IAdminUserManager
    {
        private readonly IList<string> _users = new List<string>();
        private readonly ILogger _logger;

        public AdminUserManager(ILogger logger)
        {
            _logger = logger;
        }

        public async Task AddAdminUser(string username)
        {
            if (!_users.Contains(username))
            {
                _logger.InfoFormat("Add Admin User: {0}", username);
                _users.Add(username);
            }
            await Task.Yield();
        }

        public async Task<IList<string>> GetAllAdminUser()
        {
            await Task.Yield();
            return _users;
        }

        public async Task<IList<ChatId>> GetAllAdminUserChatId()
        {
            await Task.Yield();
            return _users.Select(x => new ChatId(x)).ToArray();
        }

        public async Task RemoveAdminUser(string username)
        {
            if (_users.Contains(username))
            {
                _logger.InfoFormat("Remove Admin User: {0}", username);
                _users.Remove(username);
            }
            await Task.Yield();
        }
    }
}
