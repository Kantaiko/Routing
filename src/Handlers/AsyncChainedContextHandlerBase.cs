using System.Runtime.ExceptionServices;

namespace Kantaiko.Routing.Handlers;

public abstract class AsyncChainedContextHandlerBase<TContext> : IChainedHandler<TContext, Task<Unit>>
{
    protected TContext Context { get; private set; } = default!;

    async Task<Unit> IChainedHandler<TContext, Task<Unit>>.Handle(TContext input,
        Func<Task<Unit>> next)
    {
        Context = input;

        await BeforeHandleAsync(input);

        try
        {
            await HandleAsync(input, next);
        }
        catch (Exception exception)
        {
            await AfterHandleAsync(input, exception);
            return default;
        }

        await AfterHandleAsync(input, null);
        return default;
    }

    protected abstract Task<Unit> HandleAsync(TContext context, Func<Task<Unit>> next);

    /// <summary>
    /// Invokes before HandleAsync in order to perform pre-processing operations.
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    protected virtual Task BeforeHandleAsync(TContext context)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Invokes after HandleAsync in order to perform post-processing operations.
    /// This method also handles the exception if any.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="exception"></param>
    /// <returns></returns>
    protected virtual Task AfterHandleAsync(TContext context, Exception? exception)
    {
        if (exception is not null)
        {
            ExceptionDispatchInfo.Capture(exception).Throw();
        }

        return Task.CompletedTask;
    }
}
