using System;
using System.Threading;
using System.Threading.Tasks;

namespace MjollnirBotManager.Common.PipeLines
{
    public abstract class Handler : Handler<object>, IHandler
    {
    }

    public abstract class Handler<TVaule> : Handler<IHandler<TVaule>, TVaule>, IHandler<TVaule>
        where TVaule : class, new()
    {
    }

    public abstract class Handler<THandler, TVaule> : IHandler<THandler, TVaule>
        where THandler : IHandler<THandler, TVaule>
        where TVaule : class, new()
    {
        private THandler _next;

        public int Order { get; protected set; } = 0;

        public async Task<THandler> GetNextAsync()
        {
            await Task.Yield();
            return _next;
        }

        public async Task InvokeAsync(TVaule s, CancellationToken token)
        {
            await ProcessHandler(s, token);

            if (_next != null)
                await _next.InvokeAsync(s, token);
        }

        protected abstract Task ProcessHandler(TVaule s, CancellationToken token);

        public async Task SetNextAsync(THandler handler)
        {
            _next = handler;
            await Task.Yield();
        }

        public async Task BreakAsync()
        {
            _next = default;
            await Task.Yield();
        }
    }
}
