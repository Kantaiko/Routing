using Kantaiko.Routing.AutoRegistration;
using Kantaiko.Routing.Handlers;

namespace Kantaiko.Routing.Events;

public abstract class AsyncChainedEventHandlerBase<TEvent> :
    AsyncChainedEventHandlerBase<TEvent, IAsyncEventContext<TEvent>> { }

public abstract class AsyncChainedEventHandlerBase<TEvent, TContext> : AsyncChainedContextHandlerBase<TContext>,
    IAutoRegistrableHandler
    where TContext : IAsyncEventContext<TEvent>
{
    protected TEvent Event => Context.Event;

    protected IServiceProvider ServiceProvider => Context.ServiceProvider;
    protected CancellationToken CancellationToken => Context.CancellationToken;
}
