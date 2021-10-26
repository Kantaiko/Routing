using Kantaiko.Routing.Abstractions;
using Kantaiko.Routing.Context;

namespace Kantaiko.Routing;

public static class HandlerFactoryExtensions
{
    public static IHandler<TInput, TOutput> CreateHandler<TInput, TOutput>(this IHandlerFactory handlerFactory,
        Type handlerType, IServiceProvider serviceProvider)
    {
        return (IHandler<TInput, TOutput>) handlerFactory.CreateHandler(handlerType, serviceProvider);
    }

    public static IHandler<TInput, TOutput> CreateHandler<TInput, TOutput>(this IHandlerFactory handlerFactory,
        Type handlerType, object? input)
    {
        var serviceProvider = input is IHasServiceProvider hasServiceProvider
            ? hasServiceProvider.ServiceProvider
            : DefaultServiceProvider.Instance;

        return (IHandler<TInput, TOutput>) handlerFactory.CreateHandler(handlerType, serviceProvider);
    }
}
