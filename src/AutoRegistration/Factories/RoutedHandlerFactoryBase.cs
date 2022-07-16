using System.Collections.Immutable;
using Kantaiko.Routing.Abstractions;

namespace Kantaiko.Routing.AutoRegistration.Factories;

internal abstract class RoutedHandlerFactoryBase<THandler, TContext>
{
    protected abstract THandler CombineHandlers(IEnumerable<THandler> handlers);

    protected abstract THandler CreateTransientHandler(Type handlerType, IHandlerFactory? handlerFactory);

    protected abstract Type CreateHandlerInterfaceType(Type actualContextType);

    protected abstract Type CreatePolymorphicHandlerType(Type actualContextType);

    protected abstract THandler CreateEmptyHandler();

    protected abstract THandler CreateRouter(Dictionary<Type, THandler> routes);

    public THandler Create(IEnumerable<Type> lookupTypes, IHandlerFactory? handlerFactory)
    {
        // All interesting types (events, contexts and handlers) are non-abstract classes
        var typeCollection = lookupTypes
            .Where(x => x.IsClass && !x.IsAbstract)
            .ToArray();

        if (!typeof(TContext).IsGenericType)
        {
            // For non-generic context type, we can directly create corresponding transient handlers

            return CombineHandlers(CreateTransientHandlers(typeCollection, handlerFactory));
        }

        // If context type is generic, we possibly need to create a router that can handle all types

        var genericArguments = typeof(TContext).GetGenericArguments();

        if (genericArguments.Length != 1)
        {
            throw new InvalidOperationException("TContext must have one generic argument");
        }

        var eventBaseType = genericArguments[0];

        // Find all accessible event types inherited from event base type
        var eventTypes = typeCollection.Where(x => x.IsAssignableTo(eventBaseType)).ToArray();

        if (eventTypes.Length == 0)
        {
            throw new InvalidOperationException("At least one event type must be specified in lookup types");
        }

        if (eventTypes.Length == 1)
        {
            // If there is only one event type, we can also use all appropriate handlers directly

            return CombineHandlers(CreateTransientHandlers(typeCollection, handlerFactory));
        }

        // Otherwise, we need a router

        var contextGenericDefinition = typeof(TContext).GetGenericTypeDefinition();

        var contextImplementationTypes = typeCollection
            .Where(x => x.IsGenericTypeDefinition && IsAssignableToGenericType(x, contextGenericDefinition))
            .ToArray();

        if (contextImplementationTypes.Length == 0)
        {
            throw new InvalidOperationException(
                "At least one context implementation type must be specified in lookup types");
        }

        // We need to create a route for each event type and each context implementation type
        var routesCapacity = eventTypes.Length * contextImplementationTypes.Length;

        var routes = new Dictionary<Type, THandler>(routesCapacity);

        foreach (var eventType in eventTypes)
        {
            var contextType = contextGenericDefinition.MakeGenericType(eventType);

            var handlers = CreatePolymorphicTransientHandlers(contextType, typeCollection, handlerFactory).ToArray();

            foreach (var implementationType in contextImplementationTypes)
            {
                var keyType = implementationType.MakeGenericType(eventType);

                if (handlers.Length > 0)
                {
                    routes[keyType] = CombineHandlers(handlers);
                }
                else
                {
                    routes[keyType] = CreateEmptyHandler();
                }
            }
        }

        return CreateRouter(routes);
    }

    private IEnumerable<THandler> CreatePolymorphicTransientHandlers(
        Type actualContextType,
        IEnumerable<Type> lookupTypes,
        IHandlerFactory? handlerFactory)
    {
        var handlerType = CreateHandlerInterfaceType(actualContextType);

        var handlerTypes = lookupTypes.Where(x => x.IsAssignableTo(handlerType));

        var transientHandlerType = CreatePolymorphicHandlerType(actualContextType);

        return handlerTypes
            .Select(type => (THandler) Activator.CreateInstance(
                transientHandlerType,
                type,
                handlerFactory
            )!)
            .ToImmutableArray();
    }

    private IEnumerable<THandler> CreateTransientHandlers(
        IEnumerable<Type> lookupTypes,
        IHandlerFactory? handlerFactory)
    {
        var handlerTypes = lookupTypes.Where(x => x.IsAssignableTo(typeof(THandler)));

        return handlerTypes
            .Select(x => CreateTransientHandler(x, handlerFactory))
            .ToImmutableArray();
    }

    private static bool IsAssignableToGenericType(Type type, Type other)
    {
        var interfaces = type.GetInterfaces();

        foreach (var interfaceType in interfaces)
        {
            if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == other)
            {
                return true;
            }
        }

        if (type.BaseType is { } baseType)
        {
            if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == other)
            {
                return true;
            }

            return IsAssignableToGenericType(baseType, other);
        }

        return false;
    }
}
