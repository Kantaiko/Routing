using Kantaiko.Routing.Handlers;

namespace Kantaiko.Routing.AutoRegistration.Factories.ChainedHandlers;

internal class SyncChainedEventHandlerFactory<TContext> : RoutedChainedHandlerFactory<TContext, Unit>
{
    protected override IChainedHandler<TContext, Unit> CombineHandlers(
        IEnumerable<IChainedHandler<TContext, Unit>> handlers)
    {
        return new ChainHandler<TContext, Unit>(handlers);
    }

    protected override IChainedHandler<TContext, Unit> CreateEmptyHandler()
    {
        return EmptyHandler<TContext>.Instance;
    }
}
