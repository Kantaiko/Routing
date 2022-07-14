using Kantaiko.Routing.Context;

namespace Kantaiko.Routing.Events;

public class EventContext<TEvent> : AsyncContextBase, IEventContext<TEvent>
{
    public EventContext(TEvent @event,
        IServiceProvider? serviceProvider = null,
        CancellationToken cancellationToken = default
    ) : base(serviceProvider, cancellationToken)
    {
        Event = @event;
    }

    public TEvent Event { get; }
}
