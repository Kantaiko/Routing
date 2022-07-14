using Kantaiko.Routing.AutoRegistration;
using Kantaiko.Routing.Handlers;

namespace Kantaiko.Routing.Events;

public abstract class EventHandlerBase<TEvent> : AsyncContextHandlerBase<IEventContext<TEvent>>,
    IAutoRegistrableHandler
{
    protected TEvent Event => Context.Event;

    protected IServiceProvider ServiceProvider => Context.ServiceProvider;
    protected CancellationToken CancellationToken => Context.CancellationToken;
}
