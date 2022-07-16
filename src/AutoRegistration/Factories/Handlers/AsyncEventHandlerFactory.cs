using Kantaiko.Routing.Handlers;

namespace Kantaiko.Routing.AutoRegistration.Factories.Handlers;

internal abstract class AsyncEventHandlerFactory<TContext> : RoutedHandlerFactory<TContext, Task>
{
    protected override IHandler<TContext, Task> CreateEmptyHandler()
    {
        return EmptyHandler<TContext>.Instance;
    }
}
