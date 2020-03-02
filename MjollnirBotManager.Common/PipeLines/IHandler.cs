using System;
using System.Threading;
using System.Threading.Tasks;
using MjollnirBotManager.Common.Dependency;

namespace MjollnirBotManager.Common.PipeLines
{
    public interface IHandler : IHandler<object>
    {
    }

    public interface IHandler<TVaule> : IHandler<IHandler<TVaule>, TVaule>
        where TVaule : class, new()
    {
    }

    public interface IHandler<THandler, TVaule>
        where THandler : IHandler<THandler, TVaule>
        where TVaule: class, new()
    {
        int Order { get; }
        Task<THandler> GetNextAsync();
        Task SetNextAsync(THandler handler);
        Task InvokeAsync(TVaule s, CancellationToken token);
        Task BreakAsync();
    }
}
