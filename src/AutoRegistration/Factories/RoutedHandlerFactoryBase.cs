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
        var typeCollection = lookupTypes as ICollection<Type> ?? lookupTypes.ToArray();

        // List of types to look for events, event handlers and context implementations
        var classTypes = typeCollection
            .Where(x => x.IsClass && !x.IsAbstract)
            .ToArray();

        if (!typeof(TContext).IsGenericType)
        {
            // For non-generic context type, we can directly create corresponding transient handlers

            return CombineHandlers(CreateTransientHandlers(classTypes, handlerFactory));
        }

        // If context type is generic, we possibly need to create a router that can handle all types

        var genericArguments = typeof(TContext).GetGenericArguments();

        if (genericArguments.Length != 1)
        {
            throw new InvalidOperationException("TContext must have one generic argument");
        }

        var eventBaseType = genericArguments[0];

        // Find all accessible event types inherited from event base type
        var eventTypes = classTypes.Where(x => x.IsAssignableTo(eventBaseType)).ToArray();

        if (eventTypes.Length == 0)
        {
            throw new InvalidOperationException("At least one event type must be specified in lookup types");
        }

        var contextGenericDefinition = typeof(TContext).GetGenericTypeDefinition();

        // Find all accessible context types inherited from base context type
        var eventContextTypes = typeCollection
            .Where(x => x.IsAbstract || x.IsInterface)
            .Where(x => IsAssignableToGenericType(x, contextGenericDefinition))
            .Append(contextGenericDefinition)
            .ToArray();

        if (eventContextTypes.Length == 1 && eventTypes.Length == 1)
        {
            // If there is only one event type and one context type,
            // we can also use all appropriate handlers directly

            return CombineHandlers(CreateTransientHandlers(classTypes, handlerFactory));
        }

        // Otherwise, we need a router

        // We need to create a route for each event type
        // and each context type (which can be implemented by multiple types)
        var routesCapacity = eventTypes.Length * eventContextTypes.Length;

        var routes = new Dictionary<Type, THandler>(routesCapacity);

        foreach (var eventContextType in eventContextTypes)
        {
            var contextImplementationTypes = classTypes
                .Where(x => x.IsGenericTypeDefinition && IsAssignableToGenericType(x, eventContextType))
                .ToArray();

            if (contextImplementationTypes.Length == 0)
            {
                throw new InvalidOperationException(
                    $"At least one implementation of context type {eventContextType.Name} must " +
                    "be specified in lookup types");
            }

            foreach (var eventType in eventTypes)
            {
                Type contextType;

                // It seems that the most simple and reliable way to check generic type constraints
                // is to try to create a generic type and handle the exception

                try
                {
                    contextType = eventContextType.MakeGenericType(eventType);
                }
                catch (ArgumentException)
                {
                    continue;
                }

                var handlers = CreatePolymorphicTransientHandlers(
                    contextType,
                    classTypes,
                    handlerFactory
                ).ToArray();

                foreach (var implementationType in contextImplementationTypes)
                {
                    Type keyType;

                    // We also need to check the implementation constraints

                    try
                    {
                        keyType = implementationType.MakeGenericType(eventType);
                    }
                    catch (ArgumentException)
                    {
                        continue;
                    }

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
