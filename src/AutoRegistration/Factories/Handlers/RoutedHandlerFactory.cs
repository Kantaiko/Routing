using Kantaiko.Routing.Abstractions;
using Kantaiko.Routing.Handlers;

namespace Kantaiko.Routing.AutoRegistration.Factories.Handlers;

internal abstract class RoutedHandlerFactory<TContext, TOutput> :
    RoutedHandlerFactoryBase<IHandler<TContext, TOutput>, TContext>
{
    protected override IHandler<TContext, TOutput> CreateRouter(Dictionary<Type, IHandler<TContext, TOutput>> routes)
    {
        return new RouterHandler<TContext, TOutput>(routes);
    }

    protected override IHandler<TContext, TOutput> CreateTransientHandler(Type handlerType,
        IHandlerFactory? handlerFactory)
    {
        return new TransientHandler<TContext, TOutput>(handlerType, handlerFactory);
    }

    protected override Type CreateHandlerInterfaceType(Type actualContextType)
    {
        return typeof(IHandler<,>).MakeGenericType(actualContextType, typeof(TOutput));
    }

    protected override Type CreatePolymorphicHandlerType(Type actualContextType)
    {
        return typeof(PolymorphicTransientHandler<,,>)
            .MakeGenericType(actualContextType, typeof(TContext), typeof(TOutput));
    }
}
