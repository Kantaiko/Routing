using Kantaiko.Routing.Abstractions;
using Kantaiko.Routing.Events;
using Kantaiko.Routing.Handlers;

namespace Kantaiko.Routing.AutoRegistration;

public static class EventHandlerFactory
{
    public static IHandler<IEventContext<TEventBase>, Task<Unit>> CreateParallelEventHandler<TEventBase>(
        IEnumerable<Type> types, IHandlerFactory? handlerFactory = null) where TEventBase : class
    {
        ArgumentNullException.ThrowIfNull(types);

        return CreateEventHandler<TEventBase>(Handler.ParallelAsync, types, handlerFactory);
    }

    public static IHandler<IEventContext<TEventBase>, Task<Unit>> CreateSequentialEventHandler<TEventBase>(
        IEnumerable<Type> types, IHandlerFactory? handlerFactory = null) where TEventBase : class
    {
        ArgumentNullException.ThrowIfNull(types);

        return CreateEventHandler<TEventBase>(Handler.SequentialAsync, types, handlerFactory);
    }

    public static IHandler<IEventContext<TEventBase>, Task<Unit>> CreateChainedEventHandler<TEventBase>(
        IEnumerable<Type> types, IHandlerFactory? handlerFactory = null,
        IHandler<IEventContext<TEventBase>, Task<Unit>>? lastHandler = null) where TEventBase : class
    {
        ArgumentNullException.ThrowIfNull(types);

        return CreateEventHandler<TEventBase>(
            handlers =>
            {
                if (lastHandler is null)
                {
                    return Handler.Chain(handlers.Append(Handler.EmptyAsync<IEventContext<object>>()));
                }

                var handler = new CastHandler<IEventContext<TEventBase>,
                    IEventContext<object>, Task<Unit>>(lastHandler);

                return Handler.Chain(handlers.Append(handler));
            },
            types, handlerFactory);
    }

    private static IHandler<IEventContext<TEventBase>, Task<Unit>> CreateEventHandler<TEventBase>(
        Func<
            IEnumerable<IHandler<IEventContext<object>, Task<Unit>>>,
            IHandler<IEventContext<object>, Task<Unit>>
        > createHandler,
        IEnumerable<Type> types,
        IHandlerFactory? handlerFactory = null)
    {
        var typeCollection = AutoRegistrationUtils.MaterializeCollection(types);

        var eventTypes = AutoRegistrationUtils.GetInputTypes<TEventBase>(typeCollection).ToArray();

        if (eventTypes.Length == 1)
        {
            var handlers = AutoRegistrationUtils.CreateTransientHandlers<IEventContext<object>, Task<Unit>>(
                eventTypes[0], typeCollection, handlerFactory);

            return (IHandler<IEventContext<TEventBase>, Task<Unit>>) createHandler(handlers);
        }

        var routes = new Dictionary<Type, IHandler<IEventContext<TEventBase>, Task<Unit>>>();

        foreach (var eventType in eventTypes)
        {
            var keyType = typeof(EventContext<>).MakeGenericType(eventType);

            var handlers = AutoRegistrationUtils.CreateTransientHandlers<IEventContext<object>, Task<Unit>>(
                eventType, typeCollection, handlerFactory);

            routes[keyType] = (IHandler<IEventContext<TEventBase>, Task<Unit>>) createHandler(handlers);
        }

        return Handler.Router(routes);
    }
}
