using Kantaiko.Routing.AutoRegistration;
using Kantaiko.Routing.Handlers;

namespace Kantaiko.Routing.Events;

public abstract class AsyncEventHandlerBase<TEvent> : AsyncEventHandlerBase<TEvent, IAsyncEventContext<TEvent>> { }

public abstract class AsyncEventHandlerBase<TEvent, TContext> : AsyncContextHandlerBase<TContext>,
    IAutoRegistrableHandler
    where TContext : IAsyncEventContext<TEvent>
{
    protected TEvent Event => Context.Event;

    protected IServiceProvider ServiceProvider => Context.ServiceProvider;
    protected CancellationToken CancellationToken => Context.CancellationToken;
}
