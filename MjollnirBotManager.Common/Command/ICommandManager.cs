using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace MjollnirBotManager.Common.Command
{
    public interface ICommandManager
    {
        Task<ICommand> ExecuteAsync(Message message, CancellationToken token);
        Task<bool> IsCommandAsync(Message message);
    }
}