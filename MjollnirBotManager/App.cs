using System;
using System.Threading.Tasks;
using Castle.Core.Logging;
using Microsoft.Extensions.Configuration;
using MjollnirBotManager.Common;
using MjollnirBotManager.Common.Dependency;

namespace MjollnirBotManager
{
    internal sealed class App : IApp, ISingleton
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _config;
        private readonly IBotManager _manager;

        public App(ILogger logger, IConfiguration configuration, IBotManager manager)
        {
            _logger = logger;
            _config = configuration;
            _manager = manager;
        }

        public async Task RunAsyc()
        {
            _logger.InfoFormat("Start App...");

            AppDomain.CurrentDomain.ProcessExit += async (sender, eventArgs) => await StopAsync();
            Console.CancelKeyPress += async (sender, e) => { await StopAsync(); e.Cancel = true; };

            await _manager.StartAsync();
        }

        private async Task StopAsync()
        {
            _logger.InfoFormat("Stop App...");

            await _manager.StopAsync();
            _logger.InfoFormat("App Stop!");
        }
    }
}
