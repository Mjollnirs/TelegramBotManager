using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace MjollnirBotManager.Common.Security
{
    public class AdminUserValidator : IAdminUserValidator
    {
        private readonly IAdminUserManager _userManager;

        public AdminUserValidator(IAdminUserManager userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> IsAdmin(Message message)
        {
            return await IsAdmin(message.Chat) || await IsAdmin(message.From);
        }

        public async Task<bool> IsAdmin(ChatId chatId)
        {
            return await IsAdmin(chatId.Username);
        }

        public async Task<bool> IsAdmin(User user)
        {
            return await IsAdmin(user.Username);
        }

        public async Task<bool> IsAdmin(string username)
        {
            return (await _userManager.GetAllAdminUser()).Contains(username);
        }
    }
}
