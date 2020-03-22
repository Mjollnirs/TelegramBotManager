using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace MjollnirBotManager.Common.Security
{
    public class AdminChatValidator : IAdminChatValidator
    {
        private readonly IAdminChatManager _adminChatManager;

        public AdminChatValidator(IAdminChatManager adminChatManager)
        {
            _adminChatManager = adminChatManager;
        }

        public async Task<bool> IsAdmin(Message message)
        {
            return await IsAdmin(message.Chat) || await IsAdmin(message.From);
        }

        public async Task<bool> IsAdmin(ChatId chatId)
        {
            return await IsAdmin(chatId.Identifier);
        }

        public async Task<bool> IsAdmin(User user)
        {
            return await IsAdmin(user.Id);
        }

        public async Task<bool> IsAdmin(long id)
        {
            return (await _adminChatManager.GetAllAdminChats()).Select(x => x.Identifier).Contains(id);
        }
    }
}
