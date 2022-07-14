using System.Runtime.ExceptionServices;

namespace Kantaiko.Routing.Handlers;

public abstract class AsyncContextHandlerBase<TContext> : IHandler<TContext, Task>
{
    protected TContext Context { get; private set; } = default!;

    async Task IHandler<TContext, Task>.Handle(TContext input)
    {
        Context = input;

        await BeforeHandleAsync(input);

        try
        {
            await HandleAsync(input);
        }
        catch (Exception exception)
        {
            await AfterHandleAsync(input, exception);
            return;
        }

        await AfterHandleAsync(input, null);
    }

    protected abstract Task HandleAsync(TContext context);

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
