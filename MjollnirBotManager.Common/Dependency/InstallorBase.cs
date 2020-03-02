using System;
using System.Linq;
using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using MjollnirBotManager.Common.Command;

namespace MjollnirBotManager.Common.Dependency
{
    public abstract class InstallorBase : IWindsorInstaller
    {
        protected IWindsorContainer Container;

        public abstract void Pre();
        public abstract void Post();

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            Container = container;

            Pre();

            Container.Register(Classes.FromAssembly(GetType().Assembly)
                .Register<ITransient>()
                .LifestyleTransient());

            Container.Register(Classes.FromAssembly(GetType().Assembly)
                .Register<ISingleton>()
                .LifestyleSingleton());

            Container.Register(Classes.FromAssembly(GetType().Assembly)
                .Register<IScoped>()
                .LifestyleScoped());

            Container.Register(Classes.FromAssembly(GetType().Assembly)
                .Register<IPerThread>()
                .LifestylePerThread());

            Container.Register(Classes.FromAssembly(GetType().Assembly)
                .Register<IPooled>()
                .LifestylePerThread());

            Container.Register(Classes.FromAssembly(GetType().Assembly)
                .Register<ICommand>()
                .LifestyleTransient()
                .Configure(x => x.IsDefault()
                    .NamedAutomatically(x.Implementation.FullName.Split('.').LastOrDefault())));

            Post();
        }

        protected void RegisterTransient<TService>()
           where TService : class => RegisterTransient<TService, TService>();
        protected void RegisterTransient<TService, TImpl>()
            where TService : class
            where TImpl : TService
        {
            Container.Register(Component.For<TService>()
                .ImplementedBy<TImpl>()
                .LifestyleTransient());
        }

        protected void RegisterSingleton<TService>()
            where TService : class => RegisterSingleton<TService, TService>();
        protected void RegisterSingleton<TService, TImpl>()
            where TService : class
            where TImpl : TService
        {
            Container.Register(Component.For<TService>()
                .ImplementedBy<TImpl>()
                .LifestyleSingleton());
        }

        protected void RegisterScoped<TService>()
            where TService : class => RegisterScoped<TService, TService>();
        protected void RegisterScoped<TService, TImpl>()
            where TService : class
            where TImpl : TService
        {
            Container.Register(Component.For<TService>()
                .ImplementedBy<TImpl>()
                .LifestyleScoped());
        }

        protected void RegisterPerThread<TService>()
            where TService : class => RegisterPerThread<TService, TService>();
        protected void RegisterPerThread<TService, TImpl>()
            where TService : class
            where TImpl : TService
        {
            Container.Register(Component.For<TService>()
                .ImplementedBy<TImpl>()
                .LifestylePerThread());
        }

        protected void RegisterPooled<TService>()
            where TService : class => RegisterPooled<TService, TService>();
        protected void RegisterPooled<TService, TImpl>()
            where TService : class
            where TImpl : TService
        {
            Container.Register(Component.For<TService>()
                .ImplementedBy<TImpl>()
                .LifestylePooled());
        }
    }
}
