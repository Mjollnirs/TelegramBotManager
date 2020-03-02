using System;
using System.Threading;
using System.Threading.Tasks;
using MjollnirBotManager.Common.Command;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace MjollnirBotManager.Common
{
    public interface IBotManager
    {
        ICommand PrevCommand { get; }
        ITelegramBotClient Telegram { get; }
        Task StartAsync();
        Task StopAsync();
    }
}
