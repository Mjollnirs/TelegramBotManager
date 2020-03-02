using System;
using System.Reflection;
using Castle.MicroKernel.Registration;

namespace MjollnirBotManager.Common.Dependency
{
    internal static class Extensions
    {
        public static BasedOnDescriptor Register<T>(this FromAssemblyDescriptor descriptor)
            where T: IDependency
        {
            return descriptor
                .IncludeNonPublicTypes()
                .BasedOn<T>()
                .WithService.Self()
                .If(type => !type.GetTypeInfo().IsGenericTypeDefinition)
                .WithService.AllInterfaces();
        }
    }
}
