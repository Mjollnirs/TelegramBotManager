using System;
using Castle.MicroKernel;
using MjollnirBotManager.Common.Dependency;
using Telegram.Bot.Types;

namespace MjollnirBotManager.Common.PipeLines
{
    internal sealed class UpdatePipeLine : PipeLine<IUpdateHandler, Update>, ISingleton
    {
        public UpdatePipeLine(IKernel kernel)
            : base(kernel)
        {
        }
    }
}
