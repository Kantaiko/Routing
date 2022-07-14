using Kantaiko.Routing.Context;

namespace Kantaiko.Routing.Events;

public interface IEventContext<out TEvent> : IAsyncContext
{
    TEvent Event { get; }
}
