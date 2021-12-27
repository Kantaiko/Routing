namespace Kantaiko.Routing.Events;

public static class EventContextExtensions
{
    public static IEventContext<TEvent> WithEvent<TEvent>(this IEventContext<TEvent> context,
        TEvent @event)
    {
        ArgumentNullException.ThrowIfNull(context);

        return new EventContext<TEvent>(@event, context.ServiceProvider, context.CancellationToken);
    }

    public static IEventContext<TEvent> WithServiceProvider<TEvent>(this IEventContext<TEvent> context,
        IServiceProvider serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(context);

        return new EventContext<TEvent>(context.Event, serviceProvider, context.CancellationToken);
    }

    public static IEventContext<TEvent> WithCancellationToken<TEvent>(this IEventContext<TEvent> context,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(context);

        return new EventContext<TEvent>(context.Event, context.ServiceProvider, cancellationToken);
    }
}
