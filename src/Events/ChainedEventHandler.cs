using Kantaiko.Routing.AutoRegistration;

namespace Kantaiko.Routing.Events;

public abstract class ChainedEventHandler<TEvent> :
    IChainedHandler<IEventContext<IEvent>, Task<Unit>>,
    IAutoRegistrableHandler<TEvent>
    where TEvent : IEvent
{
    protected IEventContext<TEvent> Context { get; private set; } = null!;

    protected TEvent Event => Context.Event;
    protected IServiceProvider ServiceProvider => Context.ServiceProvider;
    protected CancellationToken CancellationToken => Context.CancellationToken;

    protected delegate Task<Unit> NextHandler(IEventContext<TEvent>? context = default);

    protected abstract Task<Unit> HandleAsync(IEventContext<TEvent> context, NextHandler next);

    async Task<Unit> IChainedHandler<IEventContext<IEvent>, Task<Unit>>.Handle(IEventContext<IEvent> input,
        Func<IEventContext<IEvent>, Task<Unit>> next)
    {
        Context = (IEventContext<TEvent>) input;

        await BeforeHandleAsync(Context);
        await HandleAsync(Context, x => next((IEventContext<IEvent>) (x ?? Context)));
        await AfterHandleAsync(Context);

        return default;
    }

    protected virtual Task BeforeHandleAsync(IEventContext<TEvent> context) => Task.CompletedTask;
    protected virtual Task AfterHandleAsync(IEventContext<TEvent> context) => Task.CompletedTask;
}
