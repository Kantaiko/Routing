using Kantaiko.Routing.Handlers;

namespace Kantaiko.Routing.AutoRegistration.Factories.Handlers;

internal class SyncEventHandlerFactory<TContext> : RoutedHandlerFactory<TContext, Unit>
{
    protected override IHandler<TContext, Unit> CombineHandlers(IEnumerable<IHandler<TContext, Unit>> handlers)
    {
        return new SequentialHandler<TContext>(handlers);
    }

    protected override IHandler<TContext, Unit> CreateEmptyHandler()
    {
        return EmptyHandler<TContext>.Instance;
    }
}
