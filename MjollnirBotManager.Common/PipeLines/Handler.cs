using System;
using System.Threading;
using System.Threading.Tasks;
using Castle.Core.Logging;

namespace MjollnirBotManager.Common.PipeLines
{
    public abstract class Handler : Handler<object>, IHandler
    {
        public Handler(ILogger logger)
            : base(logger)
        {
        }
    }

    public abstract class Handler<TVaule> : Handler<IHandler<TVaule>, TVaule>, IHandler<TVaule>
        where TVaule : class, new()
    {
        public Handler(ILogger logger)
            : base(logger)
        {
        }
    }

    public abstract class Handler<THandler, TVaule> : IHandler<THandler, TVaule>
        where THandler : IHandler<THandler, TVaule>
        where TVaule : class, new()
    {
        private CancellationTokenSource breakCancellationTokenSource;

        protected ILogger Logger { get; private set; }

        public Handler(ILogger logger)
        {
            Logger = logger;
        }

        private THandler _next;

        public int Order { get; protected set; } = 0;

        public async Task<THandler> GetNextAsync()
        {
            await Task.Yield();
            return _next;
        }

        public async Task InvokeAsync(TVaule vaule, CancellationToken token)
        {
            try
            {
                breakCancellationTokenSource = new CancellationTokenSource();
                CancellationToken _token = breakCancellationTokenSource.Token;

                await ProcessHandler(vaule, _token);

                if (_next != null && !token.IsCancellationRequested && !_token.IsCancellationRequested)
                    await _next.InvokeAsync(vaule, token);
            }
            catch (Exception ex)
            {
                Logger.DebugFormat(ex, "InvokeAsync");
            }
        }

        protected abstract Task ProcessHandler(TVaule s, CancellationToken token);

        public async Task SetNextAsync(THandler handler)
        {
            _next = handler;
            await Task.Yield();
        }

        public async Task BreakAsync()
        {
            if(breakCancellationTokenSource != null)
            {
                breakCancellationTokenSource.Cancel();
                breakCancellationTokenSource = null;
            }
            await Task.Yield();
        }
    }
}
