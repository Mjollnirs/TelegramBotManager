using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Castle.MicroKernel;
using MjollnirBotManager.Common.Dependency;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MjollnirBotManager.Common.Command
{
    internal sealed class CommandManager : ICommandManager, ISingleton
    {
        private readonly IKernel _kernel;

        public CommandManager(IKernel kernel)
        {
            _kernel = kernel;
        }

        public async Task<bool> IsCommandAsync(Message message)
        {
            if (message.Entities == null)
                return false;

            bool hasBotCommand = message.Entities.Where(x => x.Type == MessageEntityType.BotCommand).Any();
            string command = hasBotCommand ?
                message.EntityValues.ToList()[
                    message.Entities.ToList().IndexOf(
                        message.Entities.Where(x => x.Type == MessageEntityType.BotCommand).First())].TrimStart('/').Split('@').FirstOrDefault() :
                "Unkown";

            await Task.Yield();
            return hasBotCommand &&
                _kernel.HasComponent($"{command}Command");
        }

        public async Task<ICommand> ExecuteAsync(Message message, CancellationToken token)
        {
            if (!await IsCommandAsync(message))
                throw new NotSupportedException();

            string command = message.EntityValues.ToList()[
                    message.Entities.ToList().IndexOf(
                        message.Entities.Where(x => x.Type == MessageEntityType.BotCommand).First())].TrimStart('/').Split('@').FirstOrDefault();

            var commandObj = _kernel.Resolve<ICommand>($"{command}Command");

            await commandObj.Handle(message, token);
            return commandObj;
        }
    }
}
