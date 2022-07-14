using System.Collections.Immutable;
using Kantaiko.Routing.Abstractions;
using Kantaiko.Routing.Handlers;

namespace Kantaiko.Routing.AutoRegistration;

public static class AutoRegistrationUtils
{
    public static IEnumerable<Type> GetInputTypes(IEnumerable<Type> types, Type inputTypeBase)
    {
        return types.Where(x => x.IsClass && !x.IsAbstract && x.IsAssignableTo(inputTypeBase));
    }

    public static IReadOnlyCollection<T> MaterializeCollection<T>(IEnumerable<T> items)
    {
        return items as IReadOnlyCollection<T> ?? items.ToArray();
    }

    public static IReadOnlyList<IHandler<TInput, TOutput>> CreateTransientHandlers<TInput, TOutput>(
        Type inputType, IEnumerable<Type> types, IHandlerFactory? handlerFactory = null)
    {
        var handlerType = typeof(IHandler<,>).MakeGenericType(inputType, typeof(TOutput));

        var handlerTypes = types.Where(x =>
            x.IsClass && !x.IsAbstract &&
            x.IsAssignableTo(typeof(IAutoRegistrableHandler)) &&
            x.IsAssignableTo(handlerType));

        var transientHandlerType = typeof(PolymorphicTransientHandler<,,>)
            .MakeGenericType(inputType, typeof(TInput), typeof(TOutput));

        return handlerTypes
            .Select(type => (IHandler<TInput, TOutput>) Activator.CreateInstance(
                transientHandlerType,
                type,
                handlerFactory
            )!)
            .ToImmutableArray();
    }

    public static bool IsAssignableToGenericType(Type type, Type other)
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
