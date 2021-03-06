using Kantaiko.Routing.AutoRegistration;
using Kantaiko.Routing.Handlers;

namespace Kantaiko.Routing.Events;

public abstract class EventHandlerBase<TEvent> : EventHandlerBase<TEvent, IEventContext<TEvent>> { }

public abstract class EventHandlerBase<TEvent, TContext> : ContextHandlerBase<TContext>, IAutoRegistrableHandler
    where TContext : IEventContext<TEvent>
{
    protected TEvent Event => Context.Event;

    protected IServiceProvider ServiceProvider => Context.ServiceProvider;
}
