using Kantaiko.Routing.Abstractions;
using Kantaiko.Routing.Handlers;

namespace Kantaiko.Routing.AutoRegistration;

public static class EventHandlerFactory
{
    public static IHandler<TContext, Task> CreateParallelEventHandler<TContext>(
        IEnumerable<Type> lookupTypes, IHandlerFactory? handlerFactory = null)
    {
        ArgumentNullException.ThrowIfNull(lookupTypes);

        return CreateEventHandler<TContext>(
            handlers => new ParallelAsyncHandler<TContext>(handlers),
            lookupTypes,
            handlerFactory
        );
    }

    public static IHandler<TContext, Task> CreateSequentialEventHandler<TContext>(
        IEnumerable<Type> lookupTypes, IHandlerFactory? handlerFactory = null)
    {
        ArgumentNullException.ThrowIfNull(lookupTypes);

        return CreateEventHandler<TContext>(
            handlers => new SequentialAsyncHandler<TContext>(handlers),
            lookupTypes,
            handlerFactory
        );
    }

    private static IHandler<TContext, Task> CreateEventHandler<TContext>(
        Func<IEnumerable<IHandler<TContext, Task>>, IHandler<TContext, Task>> createHandler,
        IEnumerable<Type> lookupTypes,
        IHandlerFactory? handlerFactory = null)
    {
        var typeCollection = AutoRegistrationUtils.MaterializeCollection(lookupTypes);

        if (!typeof(TContext).IsGenericType)
        {
            // For non-generic context type, we can directly create corresponding transient handlers
            var handlers = HandlerFactory.CreateTransientHandlers<TContext, Task>(typeCollection, handlerFactory);

            return createHandler(handlers);
        }

        // If context type is generic, we need to

        var genericArguments = typeof(TContext).GetGenericArguments();

        if (genericArguments.Length != 1)
        {
            throw new InvalidOperationException("TContext must have one generic argument");
        }

        var eventBaseType = genericArguments[0];

        // Find all accessible event types inherited from event base type
        var eventTypes = AutoRegistrationUtils.GetInputTypes(typeCollection, eventBaseType).ToArray();

        if (eventTypes.Length == 0)
        {
            throw new InvalidOperationException("At least one event type must be specified in lookup types");
        }

        if (eventTypes.Length == 1)
        {
            // If there is only one event type, we can also use all appropriate handlers directly
            var handlers = HandlerFactory.CreateTransientHandlers<TContext, Task>(typeCollection, handlerFactory);

            return createHandler(handlers);
        }

        // Otherwise, we need a router

        var contextGenericDefinition = typeof(TContext).GetGenericTypeDefinition();

        var contextImplementationTypes = typeCollection
            .Where(x => x.IsClass && !x.IsAbstract && x.IsGenericTypeDefinition)
            .Where(x => AutoRegistrationUtils.IsAssignableToGenericType(x, contextGenericDefinition))
            .ToArray();

        if (contextImplementationTypes.Length == 0)
        {
            throw new InvalidOperationException(
                "At least one context implementation type must be specified in lookup types");
        }

        // We need to create a route for each event type and each context implementation type
        var routesCapacity = eventTypes.Length * contextImplementationTypes.Length;

        var routes = new Dictionary<Type, IHandler<TContext, Task>>(routesCapacity);

        foreach (var eventType in eventTypes)
        {
            var contextType = contextGenericDefinition.MakeGenericType(eventType);

            var handlers = AutoRegistrationUtils.CreateTransientHandlers<TContext, Task>(
                contextType, typeCollection, handlerFactory);

            foreach (var implementationType in contextImplementationTypes)
            {
                var keyType = implementationType.MakeGenericType(eventType);

                if (handlers.Count > 0)
                {
                    routes[keyType] = createHandler(handlers);
                }
                else
                {
                    routes[keyType] = Handler.EmptyAsync<TContext>();
                }
            }
        }

        return new RouterHandler<TContext, Task>(routes);
    }
}
