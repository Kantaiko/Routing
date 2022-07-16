using Kantaiko.Routing.Context;

namespace Kantaiko.Routing.Events;

public class EventContext<TEvent> : ContextBase, IEventContext<TEvent>
{
    public EventContext(TEvent @event, IServiceProvider? serviceProvider = null) : base(serviceProvider)
    {
        Event = @event;
    }

    public TEvent Event { get; }
}
