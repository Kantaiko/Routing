using Kantaiko.Routing.Abstractions;
using Kantaiko.Routing.Handlers;

namespace Kantaiko.Routing.AutoRegistration.Factories.ChainedHandlers;

internal abstract class RoutedChainedHandlerFactory<TContext, TOutput> :
    RoutedHandlerFactoryBase<IChainedHandler<TContext, TOutput>, TContext>
{
    protected override IChainedHandler<TContext, TOutput> CreateRouter(
        Dictionary<Type, IChainedHandler<TContext, TOutput>> routes)
    {
        return new ChainedRouterHandler<TContext, TOutput>(routes);
    }

    protected override IChainedHandler<TContext, TOutput> CreateTransientHandler(Type handlerType,
        IHandlerFactory? handlerFactory)
    {
        return new TransientHandler<TContext, TOutput>(handlerType, handlerFactory);
    }

    protected override Type CreateHandlerInterfaceType(Type actualContextType)
    {
        return typeof(IChainedHandler<,>).MakeGenericType(actualContextType, typeof(TOutput));
    }

    protected override Type CreatePolymorphicHandlerType(Type actualContextType)
    {
        return typeof(PolymorphicTransientHandler<,,>)
            .MakeGenericType(actualContextType, typeof(TContext), typeof(TOutput));
    }
}
