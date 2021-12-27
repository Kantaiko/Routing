using Kantaiko.Routing.Abstractions;

namespace Kantaiko.Routing.AutoRegistration;

public static class AutoRegistrationUtils
{
    public static IEnumerable<Type> GetInputTypes<TInputBase>(IEnumerable<Type> types)
    {
        return types.Where(x => x.IsClass && !x.IsAbstract && x.IsAssignableTo(typeof(TInputBase)));
    }

    public static IReadOnlyCollection<T> MaterializeCollection<T>(IEnumerable<T> items)
    {
        return items is IReadOnlyCollection<T> collection ? collection : items.ToArray();
    }

    public static IReadOnlyList<IHandler<TInput, TOutput>> CreateTransientHandlers<TInput, TOutput>(
        Type inputType, IEnumerable<Type> types, IHandlerFactory? handlerFactory = null)
    {
        var filterType = typeof(IAutoRegistrableHandler<>).MakeGenericType(inputType);

        var handlerTypes = types.Where(x =>
            x.IsClass && !x.IsAbstract &&
            x.IsAssignableTo(filterType) &&
            x.IsAssignableTo(typeof(IHandler<TInput, TOutput>)));

        return Handler.TransientRange<TInput, TOutput>(handlerTypes, handlerFactory);
    }
}
