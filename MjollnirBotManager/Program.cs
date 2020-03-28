using System.Threading.Tasks;
using Castle.Core.Logging;
using Castle.Facilities.Logging;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using LiteDB;
using Microsoft.Extensions.Configuration;
using MjollnirBotManager.Common;

namespace MjollnirBotManager
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfiguration _config = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json", true, true)
                        .AddEnvironmentVariables("APP_")
                        .AddCommandLine(args)
                        .Build();

            IWindsorContainer container = new WindsorContainer();

            container.Register(Component.For<IWindsorContainer>()
                .LifestyleSingleton()
                .Instance(container));

            container.Register(Component.For<IConfiguration>()
                .LifestyleSingleton()
                .UsingFactoryMethod(() =>
                {
                    return _config;
                }));

            container.Register(Component.For<ILiteDatabase>()
                .ImplementedBy<LiteDatabase>()
                .LifestyleSingleton()
                .UsingFactoryMethod(() =>
                {
                    return new LiteDatabase(_config.GetConnectionString("DefaultConnection"));
                }));

            container.Install(FromAssembly.InThisApplication(typeof(App).Assembly));

            container.AddFacility<LoggingFacility>(x =>
            {
                x.LogUsing<ConsoleFactory>();
            });

            container.Resolve<IApp>().RunAsyc();

            Task.Delay(-1).Wait();
        }
    }
}
