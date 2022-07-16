using Kantaiko.Routing.Context;

namespace Kantaiko.Routing.Events;

public class AsyncEventContext<TEvent> : AsyncContextBase, IAsyncEventContext<TEvent>
{
    public AsyncEventContext(TEvent @event,
        IServiceProvider? serviceProvider = null,
        CancellationToken cancellationToken = default
    ) : base(serviceProvider, cancellationToken)
    {
        Event = @event;
    }

    public TEvent Event { get; }
}
