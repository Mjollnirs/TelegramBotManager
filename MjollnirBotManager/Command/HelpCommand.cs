using System.Threading;
using System.Threading.Tasks;
using Castle.MicroKernel;
using MjollnirBotManager.Common.Command;

namespace MjollnirBotManager.Command
{
    public class HelpCommand : CommandBase
    {
        public HelpCommand(IKernel kernel)
            : base(kernel)
        {
        }

        protected override async Task Process(CancellationToken token)
        {
            await BotManager.Telegram.SendTextMessageAsync(Chat, "Hello~~~");
            await Task.Yield();
        }
    }
}
