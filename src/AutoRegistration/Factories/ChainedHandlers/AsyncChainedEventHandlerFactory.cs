using Kantaiko.Routing.Handlers;

namespace Kantaiko.Routing.AutoRegistration.Factories.ChainedHandlers;

internal class AsyncChainedEventHandlerFactory<TContext> : RoutedChainedHandlerFactory<TContext, Task<Unit>>
{
    protected override IChainedHandler<TContext, Task<Unit>> CombineHandlers(
        IEnumerable<IChainedHandler<TContext, Task<Unit>>> handlers)
    {
        return new ChainHandler<TContext, Task<Unit>>(handlers);
    }

    protected override IChainedHandler<TContext, Task<Unit>> CreateEmptyHandler()
    {
        return EmptyHandler<TContext>.Instance;
    }
}
