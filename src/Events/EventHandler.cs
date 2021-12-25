using Kantaiko.Routing.AutoRegistration;

namespace Kantaiko.Routing.Events;

public abstract class EventHandler<TEvent> :
    IHandler<IEventContext<IEvent>, Task<Unit>>,
    IAutoRegistrableHandler<TEvent>
    where TEvent : IEvent
{
    protected IEventContext<TEvent> Context { get; private set; } = null!;

    protected TEvent Event => Context.Event;
    protected IServiceProvider ServiceProvider => Context.ServiceProvider;
    protected CancellationToken CancellationToken => Context.CancellationToken;

    protected abstract Task<Unit> HandleAsync(IEventContext<TEvent> context);

    async Task<Unit> IHandler<IEventContext<IEvent>, Task<Unit>>.Handle(IEventContext<IEvent> input)
    {
        Context = (IEventContext<TEvent>) input;

        await BeforeHandleAsync(Context);
        await HandleAsync(Context);
        await AfterHandleAsync(Context);

        return default;
    }

    protected virtual Task BeforeHandleAsync(IEventContext<TEvent> context) => Task.CompletedTask;
    protected virtual Task AfterHandleAsync(IEventContext<TEvent> context) => Task.CompletedTask;
}
