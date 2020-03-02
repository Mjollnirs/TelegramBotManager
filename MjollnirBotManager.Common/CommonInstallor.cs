using System;
using Castle.MicroKernel.Registration;
using Microsoft.Extensions.Configuration;
using MjollnirBotManager.Common.Configuration;
using MjollnirBotManager.Common.Dependency;

namespace MjollnirBotManager.Common
{
    public class CommonInstallor : InstallorBase
    {
        public override void Post()
        {
            Container.Register(Component.For<IBotConfiguration>()
                .ImplementedBy<IBotConfiguration>()
                .UsingFactoryMethod(x =>
                {
                    IConfiguration root = x.Resolve<IConfiguration>();

                    BotConfiguration configuration = new BotConfiguration();
                    root.Bind("Bot", configuration);

                    return configuration;
                })
                .LifestyleTransient());
        }

        public override void Pre()
        {
        }
    }
}
