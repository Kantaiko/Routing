using Kantaiko.Routing.Context;

namespace Kantaiko.Routing.Events;

public class EventContext<TEvent> : ContextBase, IEventContext<TEvent>
{
    public EventContext(TEvent @event, IServiceProvider serviceProvider, CancellationToken cancellationToken) :
        base(serviceProvider, cancellationToken)
    {
        Event = @event;
    }

    public TEvent Event { get; }
}
