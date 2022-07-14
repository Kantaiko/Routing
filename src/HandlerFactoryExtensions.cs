using Kantaiko.Routing.Abstractions;
using Kantaiko.Routing.Context;
using Kantaiko.Routing.Handlers;

namespace Kantaiko.Routing;

public static class HandlerFactoryExtensions
{
    public static IHandler<TInput, TOutput> CreateHandler<TInput, TOutput>(this IHandlerFactory handlerFactory,
        Type handlerType, object? input = null, IServiceProvider? fallbackServiceProvider = null)
    {
        return (IHandler<TInput, TOutput>) CreateHandlerCore(
            handlerFactory,
            handlerType,
            input,
            fallbackServiceProvider
        );
    }

    public static IChainedHandler<TInput, TOutput> CreateChainedHandler<TInput, TOutput>(
        this IHandlerFactory handlerFactory, Type handlerType, object? input = null,
        IServiceProvider? fallbackServiceProvider = null)
    {
        return (IChainedHandler<TInput, TOutput>) CreateHandlerCore(
            handlerFactory,
            handlerType,
            input,
            fallbackServiceProvider
        );
    }

    private static object CreateHandlerCore(IHandlerFactory handlerFactory, Type handlerType,
        object? input = null, IServiceProvider? fallbackServiceProvider = null)
    {
        ArgumentNullException.ThrowIfNull(handlerFactory);

        fallbackServiceProvider ??= DefaultServiceProvider.Instance;

        var serviceProvider = input is IHasServiceProvider hasServiceProvider
            ? hasServiceProvider.ServiceProvider
            : fallbackServiceProvider;

        return handlerFactory.CreateHandler(handlerType, serviceProvider);
    }
}
