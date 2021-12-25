using Kantaiko.Routing.Abstractions;
using Kantaiko.Routing.AutoRegistration.Exceptions;
using Kantaiko.Routing.Events;
using Kantaiko.Routing.Requests;

namespace Kantaiko.Routing.AutoRegistration;

public static class HandlerAutoRegistrationService
{
    public static IReadOnlyList<IHandler<TInput, TOutput>> GetTransientHandlers<TInput, TOutput>(
        IEnumerable<Type> types, IHandlerFactory? handlerFactory = null)
    {
        ArgumentNullException.ThrowIfNull(types);

        return CreateTransientHandlers<TInput, TOutput>(typeof(TInput), types, handlerFactory);
    }

    public static IHandler<IEventContext<TEventBase>, Task<Unit>> CreateParallelEventHandler<TEventBase>(
        IEnumerable<Type> types, IHandlerFactory? handlerFactory = null) where TEventBase : IEvent
    {
        ArgumentNullException.ThrowIfNull(types);

        return CreateEventHandler<TEventBase>(Handler.ParallelAsync, types, handlerFactory);
    }

    public static IHandler<IEventContext<TEventBase>, Task<Unit>> CreateSequentialEventHandler<TEventBase>(
        IEnumerable<Type> types, IHandlerFactory? handlerFactory = null) where TEventBase : IEvent
    {
        ArgumentNullException.ThrowIfNull(types);

        return CreateEventHandler<TEventBase>(Handler.SequentialAsync, types, handlerFactory);
    }

    public static IHandler<IEventContext<TEventBase>, Task<Unit>> CreateChainedEventHandler<TEventBase>(
        IEnumerable<Type> types, IHandlerFactory? handlerFactory = null,
        IHandler<IEventContext<IEvent>, Task<Unit>>? lastHandler = null) where TEventBase : IEvent
    {
        ArgumentNullException.ThrowIfNull(types);

        return CreateEventHandler<TEventBase>(
            handlers => Handler.Chain(handlers.Append(lastHandler ?? Handler.EmptyAsync<IEventContext<IEvent>>())),
            types, handlerFactory);
    }

    private static IHandler<IEventContext<TEventBase>, Task<Unit>> CreateEventHandler<TEventBase>(
        Func<
            IEnumerable<IHandler<IEventContext<IEvent>, Task<Unit>>>,
            IHandler<IEventContext<IEvent>, Task<Unit>>
        > createHandler,
        IEnumerable<Type> types,
        IHandlerFactory? handlerFactory = null) where TEventBase : IEvent
    {
        var typeCollection = MaterializeCollection(types);

        var eventTypes = GetInputTypes<TEventBase>(typeCollection);
        var routes = new Dictionary<Type, IHandler<IEventContext<TEventBase>, Task<Unit>>>();

        foreach (var eventType in eventTypes)
        {
            var keyType = typeof(EventContext<>).MakeGenericType(eventType);

            var handlers = CreateTransientHandlers<IEventContext<IEvent>, Task<Unit>>(
                eventType, typeCollection, handlerFactory);

            routes[keyType] = (IHandler<IEventContext<TEventBase>, Task<Unit>>) createHandler(handlers);
        }

        return Handler.Router(routes);
    }

    public static IHandler<IRequestContext<TRequestBase>, Task<object>> CreateRequestHandler<TRequestBase>(
        IEnumerable<Type> types, IHandlerFactory? handlerFactory = null) where TRequestBase : IRequestBase
    {
        ArgumentNullException.ThrowIfNull(types);

        var typeCollection = MaterializeCollection(types);

        var inputTypes = GetInputTypes<TRequestBase>(typeCollection.Where(x => x != typeof(TRequestBase)));
        var routes = new Dictionary<Type, IHandler<IRequestContext<TRequestBase>, Task<object>>>();

        foreach (var inputType in inputTypes)
        {
            var keyType = typeof(RequestContext<>).MakeGenericType(inputType);

            var handlers = CreateTransientHandlers<IRequestContext<IRequestBase>, Task<object>>(
                inputType, typeCollection, handlerFactory);

            if (handlers.Count == 0)
            {
                throw new MissingRequestHandlerException(inputType);
            }

            if (handlers.Count > 1)
            {
                throw new AmbiguousRequestHandlerException(inputType);
            }

            routes[keyType] = (IHandler<IRequestContext<TRequestBase>, Task<object>>) handlers[0];
        }

        return Handler.Router(routes);
    }

    public static IHandler<IRequestContext<TRequestBase>, Task<object>> CreateChainedRequestHandler<TRequestBase>(
        IEnumerable<Type> types, IHandlerFactory? handlerFactory = null,
        IHandler<IRequestContext<IRequestBase>, Task<object>>? lastHandler = null) where TRequestBase : IRequestBase
    {
        ArgumentNullException.ThrowIfNull(types);

        var typeCollection = MaterializeCollection(types);

        var inputTypes = GetInputTypes<TRequestBase>(typeCollection.Where(x => x != typeof(TRequestBase)));
        var routes = new Dictionary<Type, IHandler<IRequestContext<TRequestBase>, Task<object>>>();

        foreach (var inputType in inputTypes)
        {
            var keyType = typeof(RequestContext<>).MakeGenericType(inputType);

            IEnumerable<IHandler<IRequestContext<IRequestBase>, Task<object>>> handlers =
                CreateTransientHandlers<IRequestContext<IRequestBase>, Task<object>>(inputType,
                    typeCollection, handlerFactory);

            if (lastHandler is not null)
            {
                handlers = handlers.Append(lastHandler);
            }

            routes[keyType] = (IHandler<IRequestContext<TRequestBase>, Task<object>>) Handler.Chain(handlers);
        }

        return Handler.Router(routes);
    }

    private static IEnumerable<Type> GetInputTypes<TInputBase>(IEnumerable<Type> types)
    {
        return types.Where(x => x.IsClass && !x.IsAbstract && x.IsAssignableTo(typeof(TInputBase)));
    }

    private static IReadOnlyCollection<T> MaterializeCollection<T>(IEnumerable<T> items)
    {
        return items is IReadOnlyCollection<T> collection ? collection : items.ToArray();
    }

    private static IReadOnlyList<IHandler<TInput, TOutput>> CreateTransientHandlers<TInput, TOutput>(
        Type inputType, IEnumerable<Type> types, IHandlerFactory? handlerFactory = null)
    {
        var handlerTypes = GetHandlerTypes<TInput, TOutput>(inputType, types);

        return Handler.TransientRange<TInput, TOutput>(handlerTypes, handlerFactory);
    }

    private static IEnumerable<Type> GetHandlerTypes<TInput, TOutput>(Type inputType, IEnumerable<Type> types)
    {
        var filterType = typeof(IAutoRegistrableHandler<>).MakeGenericType(inputType);

        var handlerTypes = types.Where(x =>
            x.IsClass && !x.IsAbstract &&
            x.IsAssignableTo(filterType) &&
            x.IsAssignableTo(typeof(IHandler<TInput, TOutput>)));

        return handlerTypes;
    }
}
