using Kantaiko.Routing.Abstractions;
using Kantaiko.Routing.Context;

namespace Kantaiko.Routing;

public static class HandlerFactoryExtensions
{
    public static IHandler<TInput, TOutput> CreateHandler<TInput, TOutput>(this IHandlerFactory handlerFactory,
        Type handlerType, object? input = null, IServiceProvider? fallbackServiceProvider = null)
    {
        ArgumentNullException.ThrowIfNull(handlerFactory);
        ArgumentNullException.ThrowIfNull(handlerFactory);

        fallbackServiceProvider ??= DefaultServiceProvider.Instance;

        var serviceProvider = input is IHasServiceProvider hasServiceProvider
            ? hasServiceProvider.ServiceProvider
            : fallbackServiceProvider;

        return (IHandler<TInput, TOutput>) handlerFactory.CreateHandler(handlerType, serviceProvider);
    }
}
