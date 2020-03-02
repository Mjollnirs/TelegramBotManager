using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Castle.MicroKernel;
using System.Linq;
using System.Threading;

namespace MjollnirBotManager.Common.PipeLines
{
    public abstract class PipeLine : PipeLine<object>, IPipeLine
    {
        public PipeLine(IKernel kernel)
            : base(kernel)
        {
        }
    }

    public abstract class PipeLine<TVaule> : PipeLine<IHandler<TVaule>, TVaule>, IPipeLine<TVaule>
        where TVaule : class, new()
    {
        public PipeLine(IKernel kernel)
            : base(kernel)
        {
        }
    }

    public abstract class PipeLine<THandler, TVaule> : IPipeLine<THandler, TVaule>
        where THandler : IHandler<THandler, TVaule>
        where TVaule : class, new()
    {
        public PipeLine(IKernel kernel)
        {
            _kernel = kernel;
            SetupAsync().Wait();
        }

        private readonly Stack<THandler> _handlers = new Stack<THandler>();
        private readonly IKernel _kernel;

        public virtual async Task AddHandler(THandler handler)
        {
            var next = await GetFirstAsync();
            if (next != null)
                await handler.SetNextAsync(next);
            _handlers.Push(handler);
        }

        public async Task InvokeAsync(TVaule vaule, CancellationToken token)
        {
            var handler = await GetFirstAsync();
            if (handler != null)
                await handler.InvokeAsync(vaule, token);
        }

        protected virtual async Task<THandler> GetFirstAsync()
        {
            await Task.Yield();
            return _handlers.Count != 0 ? _handlers.Peek() : default;
        }

        protected virtual async Task SetupAsync()
        {
            foreach (var item in _kernel.ResolveAll<THandler>().OrderByDescending(x => x.Order))
            {
                await AddHandler(item);
            }
        }
    }
}
