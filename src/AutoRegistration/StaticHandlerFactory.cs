using Kantaiko.Routing.Abstractions;

namespace Kantaiko.Routing.AutoRegistration;

public static class StaticHandlerFactory
{
    public static IReadOnlyList<IHandler<TInput, TOutput>> CreateTransientHandlers<TInput, TOutput>(
        IEnumerable<Type> types, IHandlerFactory? handlerFactory = null)
    {
        ArgumentNullException.ThrowIfNull(types);

        var handlerTypes = types.Where(x =>
            x.IsClass && !x.IsAbstract &&
            x.IsAssignableTo(typeof(IAutoRegistrableHandler)) &&
            x.IsAssignableTo(typeof(IHandler<TInput, TOutput>)));

        return Handler.TransientRange<TInput, TOutput>(handlerTypes, handlerFactory);
    }
}
