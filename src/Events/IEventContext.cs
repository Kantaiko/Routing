using Kantaiko.Routing.Context;

namespace Kantaiko.Routing.Events;

public interface IEventContext<out TEvent> : IHasServiceProvider, IHasCancellationToken
{
    TEvent Event { get; }
}
