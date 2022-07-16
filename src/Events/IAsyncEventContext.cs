using Kantaiko.Routing.Context;

namespace Kantaiko.Routing.Events;

public interface IAsyncEventContext<out TEvent> : IAsyncContext, IHasEvent<TEvent> { }
