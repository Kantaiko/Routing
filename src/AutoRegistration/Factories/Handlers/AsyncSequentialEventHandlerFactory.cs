using Kantaiko.Routing.Handlers;

namespace Kantaiko.Routing.AutoRegistration.Factories.Handlers;

internal class AsyncSequentialEventHandlerFactory<TContext> : AsyncEventHandlerFactory<TContext>
{
    protected override IHandler<TContext, Task> CombineHandlers(IEnumerable<IHandler<TContext, Task>> handlers)
    {
        return new SequentialAsyncHandler<TContext>(handlers);
    }
}
