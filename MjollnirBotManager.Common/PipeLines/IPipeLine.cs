using System;
using System.Threading;
using System.Threading.Tasks;
using MjollnirBotManager.Common.Dependency;

namespace MjollnirBotManager.Common.PipeLines
{
    public interface IPipeLine : IPipeLine<object>
    {
    }

    public interface IPipeLine<TVaule> : IPipeLine<IHandler<TVaule>, TVaule>
        where TVaule : class, new()
    {
    }

    public interface IPipeLine<THandler, TVaule>
        where THandler : IHandler<THandler, TVaule>
        where TVaule : class, new()
    {
        Task InvokeAsync(TVaule vaule, CancellationToken token);
        Task AddHandler(THandler handler);
    }
}
