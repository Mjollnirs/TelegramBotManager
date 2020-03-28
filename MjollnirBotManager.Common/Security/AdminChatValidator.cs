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

        public async Task<bool> IsAdminAsync(Message message)
        {
            return await IsAdminAsync(message.Chat) || await IsAdminAsync(message.From);
        }

        public async Task<bool> IsAdminAsync(ChatId chatId)
        {
            return await IsAdminAsync(chatId.Identifier);
        }

        public async Task<bool> IsAdminAsync(User user)
        {
            return await IsAdminAsync(user.Id);
        }

        public async Task<bool> IsAdminAsync(long id)
        {
            return (await _adminChatManager.GetAllAdminChatsAsync()).Select(x => x.Identifier).Contains(id);
        }
    }
}
