using System;
using System.Threading;
using System.Threading.Tasks;
using Castle.MicroKernel;
using MjollnirBotManager.Common.Command;

namespace MjollnirBotManager.Command
{
    public class StartCommand : CommandBase
    {
        public StartCommand(IKernel kernel)
            : base(kernel)
        {
        }

        protected async override Task Process(CancellationToken token)
        {
            await BotManager.Telegram.SendTextMessageAsync(Chat, "Hello~~~");
            await Task.Yield();
        }
    }
}
