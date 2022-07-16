using Kantaiko.Routing.Abstractions;
using Kantaiko.Routing.AutoRegistration.Factories.ChainedHandlers;
using Kantaiko.Routing.AutoRegistration.Factories.Handlers;
using Kantaiko.Routing.Handlers;

namespace Kantaiko.Routing.AutoRegistration;

public static class EventHandlerFactory
{
    public static IHandler<TContext, Unit> Sequential<TContext>(
        IEnumerable<Type> lookupTypes,
        IHandlerFactory? handlerFactory = null)
    {
        ArgumentNullException.ThrowIfNull(lookupTypes);

        return new SyncEventHandlerFactory<TContext>().Create(lookupTypes, handlerFactory);
    }

    public static IHandler<TContext, Task> SequentialAsync<TContext>(
        IEnumerable<Type> lookupTypes,
        IHandlerFactory? handlerFactory = null)
    {
        ArgumentNullException.ThrowIfNull(lookupTypes);

        return new AsyncSequentialEventHandlerFactory<TContext>().Create(lookupTypes, handlerFactory);
    }

    public static IHandler<TContext, Task> ParallelAsync<TContext>(
        IEnumerable<Type> lookupTypes,
        IHandlerFactory? handlerFactory = null)
    {
        ArgumentNullException.ThrowIfNull(lookupTypes);

        return new AsyncParallelEventHandlerFactory<TContext>().Create(lookupTypes, handlerFactory);
    }

    public static IChainedHandler<TContext, Unit> Chained<TContext>(
        IEnumerable<Type> lookupTypes,
        IHandlerFactory? handlerFactory = null)
    {
        ArgumentNullException.ThrowIfNull(lookupTypes);

        return new SyncChainedEventHandlerFactory<TContext>().Create(lookupTypes, handlerFactory);
    }

    public static IChainedHandler<TContext, Task<Unit>> ChainedAsync<TContext>(
        IEnumerable<Type> lookupTypes,
        IHandlerFactory? handlerFactory = null)
    {
        ArgumentNullException.ThrowIfNull(lookupTypes);

        return new AsyncChainedEventHandlerFactory<TContext>().Create(lookupTypes, handlerFactory);
    }
}
