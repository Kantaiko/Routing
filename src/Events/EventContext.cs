using Kantaiko.Properties;
using Kantaiko.Routing.Context;

namespace Kantaiko.Routing.Events;

public class EventContext<TEvent> : ContextBase, IEventContext<TEvent>
{
    public EventContext(TEvent @event,
        IServiceProvider? serviceProvider = null,
        IReadOnlyPropertyCollection? properties = null,
        CancellationToken cancellationToken = default) :
        base(serviceProvider, properties, cancellationToken)
    {
        Event = @event;
    }

    public TEvent Event { get; }
}
