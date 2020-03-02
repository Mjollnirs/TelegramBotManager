using System;
using System.Threading;
using System.Threading.Tasks;
using MjollnirBotManager.Common.Dependency;
using Telegram.Bot.Types;

namespace MjollnirBotManager.Common.Command
{
    public interface ICommand : IDependency
    {
        Task Handle(Message message, CancellationToken token);
        Task Reset();
    }
}
