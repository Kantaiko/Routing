using Kantaiko.Routing.AutoRegistration;
using Kantaiko.Routing.Handlers;

namespace Kantaiko.Routing.Events;

public abstract class ChainedEventHandlerBase<TEvent> : ChainedEventHandlerBase<TEvent, IEventContext<TEvent>> { }

public abstract class ChainedEventHandlerBase<TEvent, TContext> : ChainedContextHandlerBase<TContext>,
    IAutoRegistrableHandler
    where TContext : IEventContext<TEvent>
{
    protected TEvent Event => Context.Event;

    protected IServiceProvider ServiceProvider => Context.ServiceProvider;
}
