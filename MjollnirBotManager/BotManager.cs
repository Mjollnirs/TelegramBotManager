using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Castle.Core.Logging;
using Castle.MicroKernel;
using MjollnirBotManager.Common;
using MjollnirBotManager.Common.Command;
using MjollnirBotManager.Common.Configuration;
using MjollnirBotManager.Common.Dependency;
using MjollnirBotManager.Common.PipeLines;
using MjollnirBotManager.Common.Security;
using MjollnirBotManager.PipeLines;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using IUpdateHandler = MjollnirBotManager.Common.PipeLines.IUpdateHandler;

namespace MjollnirBotManager
{
    internal sealed class BotManager : IBotManager, ISingleton
    {
        private readonly ILogger _logger;
        private readonly IBotConfiguration _config;
        private readonly IYieldingUpdateReceiver _receiver;
        private readonly IPipeLine<IUpdateHandler, Update> _updatePipeLine;
        private readonly IAdminChatManager _adminChatManager;
        private readonly CommandMessageHandler _commandMessageHandler;

        private readonly CancellationTokenSource _cancellation = new CancellationTokenSource();

        public ITelegramBotClient Telegram { get; }

        public ICommand PrevCommand => _commandMessageHandler.PrevCommand;

        public BotManager(
            IKernel kernel,
            IBotConfiguration config,
            IAdminChatManager adminChatManager,
            IPipeLine<IUpdateHandler, Update> updatePipeLine,
            ILogger logger,
            CommandMessageHandler commandMessageHandler)
        {
            _config = config;
            _logger = logger;
            _adminChatManager = adminChatManager;
            _updatePipeLine = updatePipeLine;
            _commandMessageHandler = commandMessageHandler;

            Telegram = string.IsNullOrEmpty(_config.Proxy.Host)
                ? new TelegramBotClient(_config.ApiToken)
                : new TelegramBotClient(_config.ApiToken, new WebProxy(_config.Proxy.Host, _config.Proxy.Port));
            _receiver = new QueuedUpdateReceiver(Telegram);
            Telegram.OnReceiveError += Telegram_OnReceiveError;
            Telegram.OnReceiveGeneralError += Telegram_OnReceiveGeneralError;
        }

        private void Telegram_OnReceiveGeneralError(object sender, ReceiveGeneralErrorEventArgs e)
        {
            _logger.ErrorFormat(e.Exception, "OnReceiveGeneralError");
        }

        private void Telegram_OnReceiveError(object sender, ReceiveErrorEventArgs e)
        {
            _logger.ErrorFormat(e.ApiRequestException, "OnReceiveError");
        }

        public async Task StartAsync()
        {
            _logger.InfoFormat("Start BotManager...");

            try
            {
                var me = await Telegram.GetMeAsync();
                _logger.InfoFormat("I am user {0} and my name is {1}.", me.Id, me.Username);

                _logger.InfoFormat("Start Receiver...");
                ((QueuedUpdateReceiver)_receiver).StartReceiving(cancellationToken: _cancellation.Token);
                _ = Task.Run(async () => await ProcessUpdate(_cancellation.Token), _cancellation.Token);
            }
            catch (Exception ex)
            {
                _logger.ErrorFormat(ex, "StartAsync");
            }

            await LoadAsync();
        }

        public async Task StopAsync()
        {
            _logger.InfoFormat("Stop BotManager...");

            _logger.InfoFormat("Stop Receiver...");
            ((QueuedUpdateReceiver)_receiver).StopReceiving();
            _cancellation.Cancel();

            await Task.Yield();
        }

        private async Task ProcessUpdate(CancellationToken token)
        {
            await foreach (Update update in _receiver.YieldUpdatesAsync())
            {
                if (token.IsCancellationRequested)
                    break;

                _logger.InfoFormat("Receiver update {0} {1}.", update.Id, update.Type);

                try
                {
                    await _updatePipeLine.InvokeAsync(update, token);
                }
                catch (Exception ex)
                {
                    _logger.ErrorFormat(ex, "Process update Error.");
                }

                _logger.InfoFormat("Process update {0} {1} end.", update.Id, update.Type);
            }
        }

        private async Task LoadAsync()
        {
            await _adminChatManager.AddAdminChatAsync(_config.RootAdminChat);

            await Task.Yield();
        }
    }
}
